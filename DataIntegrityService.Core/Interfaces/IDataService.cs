using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Interfaces
{
  public interface IDataService
  {
    bool IsInitialised { get; set; }
    string Key { get; }
    DataServiceConfiguration Settings { get; set; }

    void Initialise();

    IEnumerable<IDataModel> TransformData(IEnumerable<IDataModel> data);

    Task<IDataResponse<IEnumerable<IDataModel>>> GetAllFromServer(HttpMessageHandler messageHandler, CancellationTokenSource cancellationTokenSource);
    Task<IDataResponse<IEnumerable<IDataModel>>> GetAllFromServer<T>(HttpMessageHandler messageHandler, CancellationTokenSource cancellationTokenSource);
    Task<IDataResponse<IEnumerable<IDataModel>>> GetAllFromServerByKey<T>(string key, HttpMessageHandler messageHandler, CancellationTokenSource cancellationTokenSource);
  }
}
