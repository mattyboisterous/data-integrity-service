using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Factories;
using DataIntegrityService.Core.Logging;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Models.Interfaces;
using DataIntegrityService.Core.Services.ChangeTracking.Interfaces;
using DataIntegrityService.Core.Services.ChangeTracking.Static;
using DataIntegrityService.Core.Services.Http;
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
    private StaticChangeTrackingServiceFactory StaticChangeTrackingServiceFactory { get; set; }
    private ChangeTrackingServiceFactory ChangeTrackingServiceFactory { get; set; }

    public async Task Initialise()
    {
      Logger.Info("EntryPoint", $"DataIntegrityService.Initialise() called...let's roll.");
      Logger.Info("EntryPoint", $"Initialising factories...");

      ServiceProvider = CreateServiceProvider();
      DataServiceFactory = ServiceProvider.GetService<DataServiceFactory>()!;
      WorkflowServiceFactory = ServiceProvider.GetService<WorkflowServiceFactory>()!;
      StaticChangeTrackingServiceFactory = ServiceProvider.GetService<StaticChangeTrackingServiceFactory>()!;
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

      await SynchroniseStaticData(forceRehydrateAll, token);

      await SynchroniseStates(SynchronisationMode.Push, forceRehydrateAll, user, token);

      await SynchroniseStates(SynchronisationMode.Pull, forceRehydrateAll, user, token);

      Logger.Info("EntryPoint", $"All work done.");
    }

    public async Task SynchroniseStaticData(bool forceRehydrateAll, CancellationToken token)
    {
      Logger.Info("EntryPoint", $"**************************************************");
      Logger.Info("EntryPoint", $"SynchroniseStaticData() called");
      Logger.Info("EntryPoint", $"**************************************************");

      if (!token.IsCancellationRequested)
      {
        // fetch static data change configuration...
        var serviceConfiguration = Configuration.StaticChangeTrackingService;

        Logger.Info("EntryPoint", $"Resolving static data service for '{serviceConfiguration.Key}'...");
        IStaticChangeTrackingService staticChangeTrackingDataService = StaticChangeTrackingServiceFactory.GetChangeTrackingService(serviceConfiguration);

        if (staticChangeTrackingDataService != null)
        {
          // initialise service...
          Logger.Info("EntryPoint", $"Initialising static data service...");
          await staticChangeTrackingDataService.Initialise();

          if (staticChangeTrackingDataService.IsInitialised)
          {
            // iterate over server dataset state, if no local version or version mismatch, fetch from server and overwrite locally...
            Logger.Info("EntryPoint", $"Iterating over server static datasets...");

            foreach (var serverDataSet in staticChangeTrackingDataService.ServerReferenceDataSetState)
            {
              Logger.Info("EntryPoint", $"Comparing server dataset '{serverDataSet.DatasetName}' with local...");

              // look for local match...
              var localDataSet = staticChangeTrackingDataService.LocalReferenceDataSetState.FirstOrDefault(ds => ds.DatasetName == serverDataSet.DatasetName);

              // refresh locally if we need to...
              if (forceRehydrateAll || localDataSet == null || localDataSet!.Version != serverDataSet.Version)
              {
                if (localDataSet == null)
                  Logger.Info("EntryPoint", $"No local dataset found, refresh from server...");
                else if (localDataSet!.Version != serverDataSet.Version)
                  Logger.Info("EntryPoint", $"Mismatch between local and server versions for this dataset, refresh from server...");

                // fetch configuration, then locate the matching data service...
                var staticDataServiceConfiguration = Configuration.DataServices.FirstOrDefault(ds => ds.DatasetName == serverDataSet.DatasetName);

                if (staticDataServiceConfiguration != null)
                {
                  Logger.Info("EntryPoint", $"Resolving data service for '{staticDataServiceConfiguration.DatasetName}'...");
                  var dataService = DataServiceFactory.GetDataService(staticDataServiceConfiguration);

                  Logger.Info("EntryPoint", $"Resolving workflow for '{staticDataServiceConfiguration.DatasetName}'...");
                  var workflow = WorkflowServiceFactory.GetDataWorkflow(staticDataServiceConfiguration.Pull!.DataWorkflow);

                  // initialise service...
                  Logger.Info("EntryPoint", $"Initialising data service...");
                  await dataService.Initialise();

                  // perform work using this workflow...
                  Logger.Info("EntryPoint", $"Executing workflow '{workflow.Key}'...");
                  var actionReponse = await workflow.ExecuteNonGeneric(null, dataService, token, staticDataServiceConfiguration.ModelType);

                  // flag as completed if all is well...
                  if (actionReponse.ActionSucceeded)
                  {
                    Logger.Info("EntryPoint", $"Dataset update succeeded, ensuring local version matches the server version.");

                    // ensure we have a matching local record, ensure last update and version matches the server version...
                    if (localDataSet == null)
                    {
                      localDataSet = new StaticDataChangeTrackingModel()
                      {
                        Id = serverDataSet.Id,
                        DatasetName = serverDataSet.DatasetName,
                        Version = serverDataSet.Version
                      };
                    }
                    else
                      localDataSet.Version = serverDataSet.Version;
                  }
                  //await changeTrackingService.FlagAsCompleted(pendingChange);
                  else
                  {
                    Logger.Error("EntryPoint", $"Action failed, unable to update the local static dataset.");

                    if (localDataSet == null)
                    {
                      // if the action failed and we have no matching local dataset, we have a blocker...
                      throw new InvalidOperationException($"Unable to refresh static data locally. Stopping work.");
                    }
                  }

                  Logger.Info("EntryPoint", $"Workflow complete for data service '{staticDataServiceConfiguration.DatasetName}', iterating...");
                }
                else
                  throw new InvalidOperationException($"Please ensure data service '{staticDataServiceConfiguration.DatasetName}' has been configured before calling 'Execute'.");
              }
              else
                Logger.Info("EntryPoint", $"Version match, iterating...");
            }

            Logger.Info("EntryPoint", $"All work complete for static data.");
          }
        }
      }
      else
        Logger.Info("EntryPoint", $"Action cancelled by user, returning...");
    }

    public async Task SynchroniseStates(SynchronisationMode mode, bool forceRehydrateAll, IUserProfile user, CancellationToken token)
    {
      Logger.Info("EntryPoint", $"**************************************************");
      Logger.Info("EntryPoint", $"SynchroniseStates() called with mode '{mode.ToString()}'");
      Logger.Info("EntryPoint", $"**************************************************");

      if (!token.IsCancellationRequested)
      {
        IChangeTrackingService changeTrackingService;

        if (mode == SynchronisationMode.Push)
        {
          Logger.Info("EntryPoint", $"Resolving data service for 'LocalChangeTrackingService'...");
          changeTrackingService = ChangeTrackingServiceFactory.GetChangeTrackingService(Configuration.ChangeTrackingService.LocalKey);
        }
        else
        {
          Logger.Info("EntryPoint", $"Resolving data service for 'HttpChangeTrackingService'...");
          changeTrackingService = ChangeTrackingServiceFactory.GetChangeTrackingService(Configuration.ChangeTrackingService.ServerKey);
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
                Logger.Info("EntryPoint", $"Executing workflow '{workflow.Key}'...");
                var actionReponse = await workflow.ExecuteNonGeneric(pendingChange, dataService, token, serviceConfiguration.ModelType);

                // flag as completed if all is well...
                if (actionReponse.ActionSucceeded)
                  await changeTrackingService.FlagAsCompleted(pendingChange);
                else
                {
                  Logger.Info("EntryPoint", $"Action failed, number of attempts has been incremented.");

                  // if we have a 400 Http response or attempted too many times, move to poison messages...
                  if ((actionReponse.HttpResponseCode >= 400 && actionReponse.HttpResponseCode < 500) || (pendingChange.Attempts > Configuration.ChangeTrackingService.BackOff.Count))
                  {
                    // if we have a malformed request then move to poison message queue...
                    if ((actionReponse.HttpResponseCode >= 400 && actionReponse.HttpResponseCode < 500))
                      Logger.Info("EntryPoint", $"Malformed request (Http response {actionReponse.HttpResponseCode}) - moving message to poison message collection.");

                    if (pendingChange.Attempts > Configuration.ChangeTrackingService.BackOff.Count)
                      Logger.Info("EntryPoint", $"Number of attempts ({pendingChange.Attempts}) has exceeded the configured backoff amount ({Configuration.ChangeTrackingService.BackOff.Count}) - moving message to poison message collection.");

                    await changeTrackingService.FlagAsPoison(pendingChange);
                  }
                }

                Logger.Info("EntryPoint", $"Workflow complete for data service '{serviceConfiguration.DatasetName}', iterating...");
              }
              else
                throw new InvalidOperationException($"Please ensure data service '{pendingChange.DatasetName}' has been configured before calling 'Execute'.");
            }
          }
        }
      }
      else
        Logger.Info("EntryPoint", $"Action cancelled by user, returning...");
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
      services.AddTransient<IWorkflowService, DeleteInsertAllFlow>();
      services.AddTransient<IWorkflowService, DeleteInsertAllByKeyFlow>();
      services.AddTransient<IWorkflowService, PushToServerFlow>();
      services.AddTransient<IWorkflowService, PullFromServerFlow>();

      services.AddTransient<IHttpMessageHandlerService, HttpMessageHandlerService>();

      services.AddTransient<DataServiceFactory>();
      services.AddTransient<WorkflowServiceFactory>();
      services.AddTransient<StaticChangeTrackingServiceFactory>();
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