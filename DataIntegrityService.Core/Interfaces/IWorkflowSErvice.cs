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

    Task Execute(IDataService dataService, HttpMessageHandler messageHandler, CancellationTokenSource cancellationTokenSource);
  }
}
