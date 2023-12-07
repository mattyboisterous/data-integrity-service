using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Interfaces;
using DataIntegrityService.Core.Providers;
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
      var factory = serviceProvider.GetService<DataServiceFactory>()!;

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
          var dataService = factory.GetDataService(serviceConfiguration);

          // initialise service...
          dataService.Initialise();

          // todo: determine pattern, invoke correct workflow...
          // todo: call "Execute" on workflow service...


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
      services.AddTransient<IDataService, VisitService>();
      
      services.AddTransient<DataServiceFactory>();
    }
  }
}
