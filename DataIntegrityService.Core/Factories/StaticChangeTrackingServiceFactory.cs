using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Services.ChangeTracking.Interfaces;
using DataIntegrityService.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Factories
{
  public class StaticChangeTrackingServiceFactory
  {
    private readonly IEnumerable<IStaticChangeTrackingService> _changeTrackingServices;

    public StaticChangeTrackingServiceFactory(IEnumerable<IStaticChangeTrackingService> services)
    {
      _changeTrackingServices = services;
    }
    public IStaticChangeTrackingService GetChangeTrackingService(StaticChangeTrackingConfiguration serviceConfiguration)
    {
      var changeTrackingSataService = _changeTrackingServices.FirstOrDefault(wf => wf.Key == serviceConfiguration.Key)
                  ?? throw new NotSupportedException($"Static change tracking service '{serviceConfiguration.Key}' has not been configured to run.");

      changeTrackingSataService.Settings = serviceConfiguration;

      return changeTrackingSataService;
    }
  }
}
