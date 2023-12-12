using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Interfaces;
using DataIntegrityService.Core.Mappers;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Providers;
using DataIntegrityService.Core.Services;
using DataIntegrityService.Core.Workflows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

      if (settings != null)
      {
        foreach (var serviceConfiguration in settings.DataServices)
        {
          var dataService = serviceFactory.GetDataService(serviceConfiguration);
          var workflow = workflowFactory.GetDataWorkflow(serviceConfiguration.DataWorkflow);

          // initialise service...
          dataService.Initialise();

          // determine concrete types...
          var sourceType = Type.GetType(dataService.Settings.Models.Source) as IDataModel;
          var destinationType = Type.GetType(dataService.Settings.Models.Destination) as IDataModel;
          
          //var mapper = Type.GetType(dataService.Settings.MapperKey);

          // perform work using this workflow...
          workflow.Execute(dataService, sourceType, destinationType, null, null); 
        }
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

      services.AddTransient<DataServiceFactory>();
      services.AddTransient<WorkflowServiceFactory>();
    }
  }
}
