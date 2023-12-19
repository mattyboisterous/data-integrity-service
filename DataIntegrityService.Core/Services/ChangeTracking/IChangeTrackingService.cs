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
    DataChangeTrackingModel GetNextChange();
    Task IncrementAttempt(DataChangeTrackingModel item);
    Task FlagAsCompleted(DataChangeTrackingModel item);
    Task FlagAsPoison(DataChangeTrackingModel item);
    Task FlushAllPendingChanges();
  }
}
