using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Models.Interfaces;
using DataIntegrityService.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Workflows.Interfaces
{
  public interface IWorkflowService
  {
    string Key { get; }

    Task<IActionResponse> ExecuteNonGeneric(DataChangeTrackingModel message, IDataService dataService, CancellationToken cancellationToken, string typeName);
    Task<IActionResponse> Execute<T>(DataChangeTrackingModel message, IDataService dataService, CancellationToken cancellationToken) where T : IDataModel;
  }
}
