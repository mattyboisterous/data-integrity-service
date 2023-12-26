using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Factories;
using DataIntegrityService.Core.Logging;
using DataIntegrityService.Core.Models.Interfaces;
using DataIntegrityService.Core.Providers;
using DataIntegrityService.Core.Services;
using DataIntegrityService.Core.Services.ChangeTracking;
using DataIntegrityService.Core.Services.Http;
using DataIntegrityService.Core.Services.Interfaces;
using DataIntegrityService.Core.Workflows;
using DataIntegrityService.Core.Workflows.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DataIntegrityService.Core
{
  public class EntryPoint
  {
    private ServiceConfiguration Configuration { get; }
    private DataServiceFactory DataServiceFactory { get; set; }
    private WorkflowServiceFactory WorkflowServiceFactory { get; set; }
    private ChangeTrackingServiceFactory ChangeTrackingServiceFactory { get; set; }

    public EntryPoint()
    {
      Logger.Info("EntryPoint", $"DataIntegrityService.ctor() called...let's roll. Initialising factories...");

      var serviceProvider = CreateServiceProvider();
      DataServiceFactory = serviceProvider.GetService<DataServiceFactory>()!;
      WorkflowServiceFactory = serviceProvider.GetService<WorkflowServiceFactory>()!;
      ChangeTrackingServiceFactory = serviceProvider.GetService<ChangeTrackingServiceFactory>()!;

      // Build a config object, using env vars and JSON providers.
      IConfigurationRoot config = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json")
          .AddEnvironmentVariables()
          .Build();

      // Get values from the config given their key and their target type.
      Configuration = config.GetRequiredSection("serviceConfiguration").Get<ServiceConfiguration>()!;

      Logger.Info("EntryPoint", $"Factories and configuration initialised.");
    }

    public async Task Run(IUserProfile user, CancellationToken token)
    {
      // todo: define model to hold tracked changes...DONE
      // todo: ref dataset empty? hydrate from server...(first time use)...

      // todo: fetch server tracked changes...
      // todo: push local changes to server...
      // todo: order server changes by dependency, perform work in order...

      // todo: ref service: order by dependencies (0 first)...model on existing behaviour...
      // todo: changes service > (push then pull, pull then push)
      // todo: changes service > order by Created (Utc)
      // todo: iterate, calling appropriate provider, workflow...execute...

      // todo: consider poison message queue, exponential backoff on certain Http responses...

      // todo: consider how multi user/secondary officer would work...on switch user, update any pending changes on server with userId?
      // todo: consider newly switched user...how to initialise local state based on all open visits etc...hwo to make application agnostic?
      // todo: need this for a fresh user...'initialise'...check DIS for current implementation...

      // todo: ref data first, always...
      // todo: flag to hydrate all, pass in variable?

      // ReferenceDataChangeService
      // todo: get both local and server...
      // todo: iterate over server...on change or no local, get data server and workflow baby! Execute!!!!
      // todo: there is only one data service we know we need...intsantiate it...
      // todo: .Initialise() gets dataset from both local and server...
      // todo: service needs public collections for both local and server...if local is empty or we are refreshing, do it!

      if (!token.IsCancellationRequested)
      {
        Logger.Info("EntryPoint", $"Resolving data service for 'LocalChangeTrackingService'...");
        //IDataService referenceDataService = DataServiceFactory.GetDataService("LocalChangeTrackingService");
      }
      
      if (!token.IsCancellationRequested)
      {
        await SynchroniseStates(SynchronisationMode.Push, user, token);
      }

      if (!token.IsCancellationRequested)
      {
        await SynchroniseStates(SynchronisationMode.Pull, user, token);
      }

      Logger.Info("EntryPoint", $"All work done! Stopping...");
    }

    public async Task SynchroniseStates(SynchronisationMode mode, IUserProfile user, CancellationToken token)
    {
      IChangeTrackingService changeTrackingService;

      if (mode == SynchronisationMode.Push)
      {
        Logger.Info("EntryPoint", $"Resolving data service for 'LocalChangeTrackingService'...");
        changeTrackingService = ChangeTrackingServiceFactory.GetChangeTrackingService("LocalChangeTrackingService");
      }
      else
      {
        Logger.Info("EntryPoint", $"Resolving data service for 'HttpChangeTrackingService'...");
        changeTrackingService = ChangeTrackingServiceFactory.GetChangeTrackingService("HttpChangeTrackingService");
      }

      // fetch tracked changes...
      changeTrackingService.Initialise();

      if (changeTrackingService.IsInitialised)
      {
        // 'compress' changes...
        await changeTrackingService.CompressPendingChanges();

        while (changeTrackingService.ChangesExist() && !token.IsCancellationRequested)
        {
          var pendingChange = changeTrackingService.GetNextChange();

          if (pendingChange != null)
          {
            // we are attempting to process you...
            await changeTrackingService.IncrementAttempt(pendingChange);

            // fetch configuration, then call up matching data service...
            var serviceConfiguration = Configuration.DataServices.FirstOrDefault(ds => ds.DatasetName == pendingChange.DatasetName);

            if (serviceConfiguration != null)
            {
              Logger.Info("EntryPoint", $"Resolving data service for '{serviceConfiguration.DatasetName}'...");
              var dataService = DataServiceFactory.GetDataService(serviceConfiguration);

              Logger.Info("EntryPoint", $"Resolving workflow for '{serviceConfiguration.DataWorkflow}'...");
              var workflow = WorkflowServiceFactory.GetDataWorkflow(serviceConfiguration.DataWorkflow);

              // initialise service...
              Logger.Info("EntryPoint", $"Initialising data service...");
              dataService.Initialise();

              // perform work using this workflow...
              Logger.Info("EntryPoint", $"Performing workflow...");
              var actionReponse = await workflow.Execute(pendingChange, dataService, token);

              // flag as completed if all is well...
              if (actionReponse.ActionSucceeded)
                await changeTrackingService.FlagAsCompleted(pendingChange);
              else
              {
                // if we have a 400 Http response or attempted too many times, move to poison messages...
                if ((actionReponse.HttpResponseCode >= 400 && actionReponse.HttpResponseCode < 500) || (pendingChange.Attempts > Configuration.ChangeTrackingService.BackOff.Count))
                {
                  await changeTrackingService.FlagAsPoison(pendingChange);
                }
              }

              Logger.Info("EntryPoint", $"Workflow complete for data service '{serviceConfiguration.DatasetName}', iterating...");
              Logger.Info("EntryPoint", "");
            }
          }
        }
      }
    }

    public IServiceProvider CreateServiceProvider()
    {
      var host = Host.CreateDefaultBuilder()
          .ConfigureServices(ConfigureServices)
          .Build();

      return host.Services;
    }

    private void ConfigureServices(IServiceCollection services)
    {
      services.AddTransient<IDataService, StaticChangeTrackingService>();
      services.AddTransient<IDataService, EvidenceNoteService>();
      services.AddTransient<IDataService, MemoService>();
      services.AddTransient<IDataService, ProvisionService>();
      services.AddTransient<IDataService, QualityAreaService>();
      services.AddTransient<IDataService, VisitService>();

      services.AddTransient<IWorkflowService, DeleteInsertAllFlow>();
      services.AddTransient<IWorkflowService, DeleteInsertAllByKeyFlow>();
      services.AddTransient<IWorkflowService, PushToServerFlow>();

      services.AddTransient<IHttpService, MockHttpService>();
      services.AddTransient<IHttpMessageHandlerService, HttpMessageHandlerService>();

      services.AddTransient<DataServiceFactory>();
      services.AddTransient<WorkflowServiceFactory>();
      services.AddTransient<ChangeTrackingServiceFactory>();
    }
  }
}
