using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Interfaces
{
  public interface IDataService : IService
  {
    DataServiceConfiguration Settings { get; set; }

    IEnumerable<IDataModel> TransformData(IEnumerable<IDataModel> data);

    Task<IDataResponse<IEnumerable<IDataModel>>> GetAllFromServer(HttpMessageHandler messageHandler, CancellationTokenSource cancellationTokenSource);
    Task<IDataResponse<IEnumerable<IDataModel>>> GetAllFromServerByKey(string key, HttpMessageHandler messageHandler, CancellationTokenSource cancellationTokenSource);
  }
}
