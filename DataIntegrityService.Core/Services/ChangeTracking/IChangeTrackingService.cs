using DataIntegrityService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Services.ChangeTracking
{
  public interface IChangeTrackingService : IService
  {
    AllModelDataSetChange GetNextChange();
    Task IncrementAttempt(AllModelDataSetChange item);
    Task FlagAsCompleted(AllModelDataSetChange item);
    Task FlagAsPoison(AllModelDataSetChange item);
    Task FlushAllPendingChanges();
  }
}
