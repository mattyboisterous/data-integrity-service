using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Models.Interfaces;
using DataIntegrityService.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Services.ChangeTracking.Interfaces
{
  public interface IStaticChangeTrackingService : IService
  {
    StaticChangeTrackingConfiguration Settings { get; set; }
    bool ForceRehydrateAll { get; set; }

    List<StaticDataChangeTrackingModel> LocalReferenceDataSetState { get; set; }
    List<StaticDataChangeTrackingModel> ServerReferenceDataSetState { get; set; }

    Task<IDataResponse<IEnumerable<StaticDataChangeTrackingModel>>> GetAllFromServer(CancellationToken cancellationToken);

    IEnumerable<StaticDataChangeTrackingModel> GetAllLocal(string key);
    int UpsertLocal(string key, IEnumerable<StaticDataChangeTrackingModel> items);
  }
}
