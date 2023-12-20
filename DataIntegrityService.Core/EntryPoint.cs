using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Interfaces;
using DataIntegrityService.Core.Logging;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Providers;
using DataIntegrityService.Core.Services;
using DataIntegrityService.Core.Services.Http;
using DataIntegrityService.Core.Workflows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DataIntegrityService.Core
{
  public class EntryPoint
  {
    public static void Run(IUserProfile user, CancellationToken token)
    {
      var serviceProvider = CreateServiceProvider();
      var serviceFactory = serviceProvider.GetService<DataServiceFactory>()!;
      var workflowFactory = serviceProvider.GetService<WorkflowServiceFactory>()!;
      var changeTrackingFactory = serviceProvider.GetService<ChangeTrackingServiceFactory>()!;

      // Build a config object, using env vars and JSON providers.
      IConfigurationRoot config = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json")
          .AddEnvironmentVariables()
          .Build();

      // Get values from the config given their key and their target type.
      ServiceConfiguration? settings = config.GetRequiredSection("serviceConfiguration").Get<ServiceConfiguration>();

      Logger.Info("EntryPoint", $"Resolving data service for 'LocalChangeTrackingService'...");
      var localChangeTrackingService = changeTrackingFactory.GetChangeTrackingService("LocalChangeTrackingService");

      // fetch local tracked changes...
      localChangeTrackingService.Initialise();

      if (localChangeTrackingService.IsInitialised)
      {
        // 'compress' changes...
        localChangeTrackingService.CompressPendingChanges();

        while (localChangeTrackingService.ChangesExist() && !token.IsCancellationRequested)
        {
          var pendingChange = localChangeTrackingService.GetNextChange();

          if (pendingChange != null)
          {
            // fetch configuration, then call up matching data service...
            var serviceConfiguration = settings!.DataServices.FirstOrDefault(ds => ds.DatasetName == pendingChange.DatasetName);

            if (serviceConfiguration != null)
            {
              Logger.Info("EntryPoint", $"Resolving data service for '{serviceConfiguration.DatasetName}'...");
              var dataService = serviceFactory.GetDataService(serviceConfiguration);

              Logger.Info("EntryPoint", $"Resolving workflow for '{serviceConfiguration.DataWorkflow}'...");
              var workflow = workflowFactory.GetDataWorkflow(serviceConfiguration.DataWorkflow);

              // initialise service...
              Logger.Info("EntryPoint", $"Initialising data service...");
              dataService.Initialise();

              // perform work using this workflow...
              Logger.Info("EntryPoint", $"Performing workflow...");
              workflow.Execute(dataService, null, token);

              Logger.Info("EntryPoint", $"Workflow complete for data service '{serviceConfiguration.DatasetName}', iterating...");
              Logger.Info("EntryPoint", "");
            }
          }
        }
      }

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

      if (settings != null)
      {
        Logger.Info("EntryPoint", " *** Data Integrity Service running ***");
        Logger.Info("EntryPoint", "");



        foreach (var serviceConfiguration in settings.DataServices)
        {
          Logger.Info("EntryPoint", $"Resolving data service for '{serviceConfiguration.DatasetName}'...");
          var dataService = serviceFactory.GetDataService(serviceConfiguration);

          Logger.Info("EntryPoint", $"Resolving workflow for '{serviceConfiguration.DataWorkflow}'...");
          var workflow = workflowFactory.GetDataWorkflow(serviceConfiguration.DataWorkflow);

          // initialise service...
          Logger.Info("EntryPoint", $"Initialising data service...");
          dataService.Initialise();

          // perform work using this workflow...
          Logger.Info("EntryPoint", $"Performing workflow...");
          workflow.Execute(dataService, null, token);

          Logger.Info("EntryPoint", $"Workflow complete for data service '{serviceConfiguration.DatasetName}', iterating...");
          Logger.Info("EntryPoint", "");
        }

        Logger.Info("EntryPoint", $"All work done! Stopping...");
      }
    }

    public static IServiceProvider CreateServiceProvider()
    {
      var host = Host.CreateDefaultBuilder()
          .ConfigureServices(ConfigureServices)
          .Build();

      return host.Services;
    }

    private static void ConfigureServices(IServiceCollection services)
    {
      services.AddTransient<IDataService, EvidenceNoteService>();
      services.AddTransient<IDataService, MemoService>();
      services.AddTransient<IDataService, ProvisionService>();
      services.AddTransient<IDataService, QualityAreaService>();
      services.AddTransient<IDataService, VisitService>();

      services.AddTransient<IWorkflowService, DeleteInsertAllFlow>();
      services.AddTransient<IWorkflowService, DeleteInsertAllByKeyFlow>();

      services.AddTransient<IHttpService, MockHttpService>();
      services.AddTransient<IHttpMessageHandlerService, HttpMessageHandlerService>();

      services.AddTransient<DataServiceFactory>();
      services.AddTransient<WorkflowServiceFactory>();
      services.AddTransient<ChangeTrackingServiceFactory>();
    }
  }
}
