using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Services.ChangeTracking.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Services.ChangeTracking
{
  public class MockHttpChangeTrackingService : IChangeTrackingService
  {
    public bool IsInitialised { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public string Key => throw new NotImplementedException();

    public bool ChangesExist()
    {
      throw new NotImplementedException();
    }

    public Task CompressPendingChanges()
    {
      throw new NotImplementedException();
    }

    public Task FlagAsCompleted(DataChangeTrackingModel item)
    {
      throw new NotImplementedException();
    }

    public Task FlagAsPoison(DataChangeTrackingModel item)
    {
      throw new NotImplementedException();
    }

    public Task FlushAllPendingChanges()
    {
      throw new NotImplementedException();
    }

    public DataChangeTrackingModel GetNextChange()
    {
      throw new NotImplementedException();
    }

    public Task IncrementAttempt(DataChangeTrackingModel item)
    {
      throw new NotImplementedException();
    }

    public Task Initialise()
    {
      throw new NotImplementedException();
    }
  }
}
