using DataIntegrityService.Core.Interfaces;
using DataIntegrityService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Workflows
{
  public class PushUpdateFlow : IWorkflowService
  {
    public string Key => "PushUpdate";

    public Task<IActionResponse> Execute(DataChangeTrackingModel message, IDataService dataService, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }
  }
}
