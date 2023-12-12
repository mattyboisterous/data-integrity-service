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

    Task Execute<T1, T2>(IDataService dataService, T1 sourceType, T2 destinationType, HttpMessageHandler messageHandler, CancellationTokenSource cancellationTokenSource);
  }
}
