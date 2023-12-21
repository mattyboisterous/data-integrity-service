using DataIntegrityService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Interfaces
{
  public interface IWorkflowService
  {
    string Key { get; }

    Task<IActionResponse> Execute(DataChangeTrackingModel message, IDataService dataService, CancellationToken cancellationToken);
  }
}
