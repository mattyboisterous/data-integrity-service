using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Services.ChangeTracking.Interfaces
{
  public interface IChangeTrackingService : IService
  {
    List<DataChangeTrackingModel> TrackedChanges { get; set; }

    bool ChangesExist();
    Task CompressPendingChanges();
    DataChangeTrackingModel GetNextChange();
    Task IncrementAttempt(DataChangeTrackingModel item);
    Task FlagAsCompleted(DataChangeTrackingModel item);
    Task FlagAsPoison(DataChangeTrackingModel item);
    Task FlushAllPendingChanges();
  }
}
