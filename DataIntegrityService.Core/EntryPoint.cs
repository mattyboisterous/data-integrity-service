using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Factories;
using DataIntegrityService.Core.Logging;
using DataIntegrityService.Core.Models.Interfaces;
using DataIntegrityService.Core.Services;
using DataIntegrityService.Core.Services.ChangeTracking;
using DataIntegrityService.Core.Services.ChangeTracking.Interfaces;
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
    private IServiceProvider ServiceProvider { get; set; }
    private IServiceCollection ExternalDependencies { get; set; }
    private ServiceConfiguration Configuration { get; set; }
    private DataServiceFactory DataServiceFactory { get; set; }
    private WorkflowServiceFactory WorkflowServiceFactory { get; set; }
    private ChangeTrackingServiceFactory ChangeTrackingServiceFactory { get; set; }

    public async Task Initialise()
    {
      Logger.Info("EntryPoint", $"DataIntegrityService.Initialise() called...let's roll. Initialising factories...");

      ServiceProvider = CreateServiceProvider();
      DataServiceFactory = ServiceProvider.GetService<DataServiceFactory>()!;
      WorkflowServiceFactory = ServiceProvider.GetService<WorkflowServiceFactory>()!;
      ChangeTrackingServiceFactory = ServiceProvider.GetService<ChangeTrackingServiceFactory>()!;

      // Build a config object, using env vars and JSON providers.
      IConfigurationRoot config = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json")
          .AddEnvironmentVariables()
          .Build();

      // Get values from the config given their key and their target type.
      Configuration = config.GetRequiredSection("serviceConfiguration").Get<ServiceConfiguration>()!;

      Logger.Info("EntryPoint", $"Factories and configuration initialised.");

      await Task.CompletedTask;
    }

    public async Task Run(IUserProfile user, bool forceRehydrateAll, CancellationToken token)
    {
      Logger.Info("EntryPoint", $"Let's do some work....");

      // todo: order server changes by dependency, perform work in order...

      // todo: consider how multi user/secondary officer would work...on switch user, update any pending changes on server with userId?
      // todo: consider newly switched user...how to initialise local state based on all open visits etc...hwo to make application agnostic?
      // todo: need this for a fresh user...'initialise'...check DIS for current implementation...

      //await SynchroniseStaticData(forceRehydrateAll, token);

      await SynchroniseStates(SynchronisationMode.Push, user, token);

      await SynchroniseStates(SynchronisationMode.Pull, user, token);

      Logger.Info("EntryPoint", $"All work done! Stopping...");
    }

    public async Task SynchroniseStaticData(bool forceRehydrateAll, CancellationToken token)
    {
      if (!token.IsCancellationRequested)
      {
        // fetch configuration, then call up matching data service...
        var serviceConfiguration = Configuration.DataServices.FirstOrDefault(ds => ds.DatasetName == Configuration.ReferenceDataService);

        if (serviceConfiguration != null)
        {
          Logger.Info("EntryPoint", $"Resolving data service for '{Configuration.ReferenceDataService}'...");
          IDataService referenceDataService = DataServiceFactory.GetDataService(serviceConfiguration);

          if (referenceDataService != null)
          {
            // initialise service...
            Logger.Info("EntryPoint", $"Initialising reference data service...");
            await referenceDataService.Initialise();

            if (referenceDataService.IsInitialised)
            {
              // iterate over server dataset state, if no local version or version mismatch, fetch from server and overwrite locally...
              foreach (var serverDataSet in ((IStaticDataService)referenceDataService).ServerReferenceDataSetState)
              {
                // look for local match...
                var localDataSet = ((IStaticDataService)referenceDataService).LocalReferenceDataSetState.FirstOrDefault(ds => ds.DatasetName == serverDataSet.DatasetName);

                // refresh locally if we need to...
                if (forceRehydrateAll || localDataSet == null || localDataSet!.Version != serverDataSet.Version)
                {
                  Logger.Info("EntryPoint", $"Resolving workflow for '{serviceConfiguration.Pull!.DataWorkflow}'...");
                  var workflow = WorkflowServiceFactory.GetDataWorkflow(serviceConfiguration.Pull!.DataWorkflow);

                  // perform work using this workflow...
                  Logger.Info("EntryPoint", $"Performing workflow...");
                  var actionReponse = await workflow.ExecuteNonGeneric(null, referenceDataService, token, serviceConfiguration.ModelType);
                }
              }
            }
          }
        }
      }
      else
        Logger.Info("EntryPoint", $"Action cancelled by user, returning...");
    }

    public async Task SynchroniseStates(SynchronisationMode mode, IUserProfile user, CancellationToken token)
    {
      //var dataType = new Type[] { typeof(string) };
      //var genericBase = typeof(List<>);
      //var combinedType = genericBase.MakeGenericType(dataType);
      //dynamic listStringInstance = Activator.CreateInstance(combinedType);
      //listStringInstance.Add("Hello World");

      if (!token.IsCancellationRequested)
      {
        IChangeTrackingService changeTrackingService;

        if (mode == SynchronisationMode.Push)
        {
          Logger.Info("EntryPoint", $"Resolving data service for 'LocalChangeTrackingService'...");
          changeTrackingService = ChangeTrackingServiceFactory.GetChangeTrackingService("MockLocalChangeTrackingService");
        }
        else
        {
          Logger.Info("EntryPoint", $"Resolving data service for 'HttpChangeTrackingService'...");
          changeTrackingService = ChangeTrackingServiceFactory.GetChangeTrackingService("MockHttpChangeTrackingService");
        }

        // fetch tracked changes...
        await changeTrackingService.Initialise();

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

                Logger.Info("EntryPoint", $"Resolving workflow for '{serviceConfiguration.DatasetName}'...");
                var workflow = WorkflowServiceFactory.GetDataWorkflow(mode == SynchronisationMode.Push ? serviceConfiguration.Push!.DataWorkflow : serviceConfiguration.Pull!.DataWorkflow);

                // initialise service...
                Logger.Info("EntryPoint", $"Initialising data service...");
                await dataService.Initialise();

                // perform work using this workflow...
                Logger.Info("EntryPoint", $"Performing workflow...");
                var actionReponse = await workflow.ExecuteNonGeneric(pendingChange, dataService, token, serviceConfiguration.ModelType);

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
    }

    public IServiceProvider CreateServiceProvider()
    {
      var host = Host.CreateDefaultBuilder()
          .ConfigureServices(ConfigureBaseServices)
          .Build();

      return host.Services;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      ExternalDependencies = services;
    }

    private void ConfigureBaseServices(IServiceCollection services)
    {
      services.AddTransient<IChangeTrackingService, MockLocalChangeTrackingService>();
      services.AddTransient<IChangeTrackingService, MockHttpChangeTrackingService>();

      services.AddTransient<IDataService, StaticChangeTrackingService>();

      services.AddTransient<IWorkflowService, DeleteInsertAllFlow>();
      services.AddTransient<IWorkflowService, DeleteInsertAllByKeyFlow>();
      services.AddTransient<IWorkflowService, PushToServerFlow>();
      services.AddTransient<IWorkflowService, PullFromServerFlow>();

      services.AddTransient<IHttpMessageHandlerService, HttpMessageHandlerService>();

      services.AddTransient<DataServiceFactory>();
      services.AddTransient<WorkflowServiceFactory>();
      services.AddTransient<ChangeTrackingServiceFactory>();

      if (ExternalDependencies != null && ExternalDependencies.Count > 0)
      {
        foreach (var dependency in ExternalDependencies)
        {
          services.Add(dependency);
        }
      }
    }
  }
}