﻿using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Interfaces;
using DataIntegrityService.Core.Logging;
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
    public static void Run()
    {
      var serviceProvider = CreateServiceProvider();
      var serviceFactory = serviceProvider.GetService<DataServiceFactory>()!;
      var workflowFactory = serviceProvider.GetService<WorkflowServiceFactory>()!;

      // Build a config object, using env vars and JSON providers.
      IConfigurationRoot config = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json")
          .AddEnvironmentVariables()
          .Build();

      // Get values from the config given their key and their target type.
      ServiceConfiguration? settings = config.GetRequiredSection("serviceConfiguration").Get<ServiceConfiguration>();

      // todo: define model to hold tracked changes...
      // todo: ref dataset empty? hydrate from server...(first time use)...
      // todo: fetch local tracked changes...
      // todo: fetch server tracked changes...
      // todo: push local changes to server...
      // todo: order server changes by dependency, perform work in order...

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
          workflow.Execute(dataService, null, null);

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

      services.AddTransient<DataServiceFactory>();
      services.AddTransient<WorkflowServiceFactory>();
    }
  }
}
