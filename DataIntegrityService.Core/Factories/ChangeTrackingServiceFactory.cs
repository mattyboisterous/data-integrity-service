﻿using DataIntegrityService.Core.Services.ChangeTracking;

namespace DataIntegrityService.Core.Factories
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
