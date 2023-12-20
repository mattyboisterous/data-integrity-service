using DataIntegrityService.Core.Interfaces;
using DataIntegrityService.Core.Services.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core
{
  public class ChangeTrackingServiceFactory
  {
    private readonly IEnumerable<IChangeTrackingService> _changeTrackingServices;

    public ChangeTrackingServiceFactory(IEnumerable<IChangeTrackingService> services)
    {
      _changeTrackingServices = services;
    }
    public IChangeTrackingService GetChangeTrackingService(string key)
    {
      return _changeTrackingServices.FirstOrDefault(wf => wf.Key == key)
                  ?? throw new NotSupportedException();
    }
  }
}
