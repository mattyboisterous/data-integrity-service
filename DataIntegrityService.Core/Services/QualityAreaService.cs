using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Interfaces;
using DataIntegrityService.Core.Logging;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Services.Http;
using DataIntegrityService.Core.Services.Local;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataIntegrityService.Core.Services
{
  public class QualityAreaService : IDataService, ILocalDbService
  {
    private IHttpService HttpService { get; set; }

    public string Key => "QualityArea";

    public required DataServiceConfiguration Settings { get; set; }

    public bool IsInitialised { get; set; }

    public required string Url { get; set; }

    public QualityAreaService(IHttpService httpService)
    { 
      HttpService = httpService;
    }

    #region IDataService Members

    public void Initialise()
    {
      Url = Settings.Http.Get!;
      IsInitialised = true;

      Logger.Info("Data service initialised.");
    }

    public IEnumerable<IDataModel> TransformData(IEnumerable<IDataModel> data)
    {
      // no transformation required...
      Logger.Info("No transformation required, returning data.");
      return data;
    }

    public async Task<IDataResponse<IEnumerable<IDataModel>>> GetAllFromServer(HttpMessageHandler messageHandler, CancellationTokenSource cancellationTokenSource)
    {
      var result = await HttpService.GetAll<QualityAreaModel>(Url, messageHandler, cancellationTokenSource);

      return new DataResponse<IEnumerable<IDataModel>>
      {
        Data = result.Data.Cast<IDataModel>(),
        MethodSucceeded = result.MethodSucceeded
      };
    }

    public Task<IDataResponse<IEnumerable<IDataModel>>> GetAllFromServer<T>(HttpMessageHandler messageHandler, CancellationTokenSource cancellationTokenSource)
    {
      throw new NotImplementedException();
    }

    public Task<IDataResponse<IEnumerable<IDataModel>>> GetAllFromServerByKey<T>(string key, HttpMessageHandler messageHandler, CancellationTokenSource cancellationTokenSource)
    {
      throw new NotImplementedException();
    }

    #endregion

    #region ILocalDbService

    public int InsertAll(IEnumerable<IDataModel> data)
    {
      // mock response for now...
      Logger.Info($"Inserting {data.Count()} item(s) into local DB.");
      return data.Count();
    }

    public int DeleteAll<T>()
    {
      // mock response for now...
      Logger.Info($"Deleting all from local DB.");
      return 3;
    }

    #endregion
  }
}
