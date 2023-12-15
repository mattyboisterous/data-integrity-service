using DataIntegrityService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Services.ChangeTracking
{
  public class HttpChangeTrackingService : IChangeTrackingService
  {
    public string Key => "HttpChangeTrackingService";
    public bool IsInitialised { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void Initialise()
    {
      throw new NotImplementedException();
    }

    public Task FlagAsCompleted(AllModelDataSetChange item)
    {
      throw new NotImplementedException();
    }

    public Task FlagAsPoison(AllModelDataSetChange item)
    {
      throw new NotImplementedException();
    }

    public Task FlushAllPendingChanges()
    {
      throw new NotImplementedException();
    }

    public AllModelDataSetChange GetNextChange()
    {
      throw new NotImplementedException();
    }

    public Task IncrementAttempt(AllModelDataSetChange item)
    {
      throw new NotImplementedException();
    }
  }
}
