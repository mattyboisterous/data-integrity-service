using DataIntegrityService.Console.Models;
using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Logging;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Models.Interfaces;
using DataIntegrityService.Core.Services.Http;
using DataIntegrityService.Core.Services.Interfaces;
using DataIntegrityService.Core.Services.Local;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataIntegrityService.Console.Services
{
  public class QualityAreaService : IDataService
  {
    private IHttpService HttpService { get; set; }
    private IHttpMessageHandlerService HttpMessageHandlerService { get; set; }
    private ILocalCacheService CacheService { get; set; }

    public string Key => "QualityArea";

    public required DataServiceConfiguration Settings { get; set; }

    public bool IsInitialised { get; set; }

    public required string Url { get; set; }

    public QualityAreaService(
      IHttpService httpService,
      IHttpMessageHandlerService httpMessageHandlerService,
      ILocalCacheService cacheService)
    {
      HttpService = httpService;
      HttpMessageHandlerService = httpMessageHandlerService;
      CacheService = cacheService;
    }

    #region IDataService Members

    public async Task Initialise()
    {
      Url = Settings.Http.Get!;
      IsInitialised = true;

      Logger.Info("QualityAreaService", "Data service initialised.");
      await Task.CompletedTask;
    }

    public IDataModel TransformData(IDataModel data)
    {
      // no transformation required...
      Logger.Info("QualityAreaService", "No transformation required, returning data as is.");
      return data;
    }

    public IEnumerable<IDataModel> TransformData(IEnumerable<IDataModel> data)
    {
      // no transformation required...
      Logger.Info("QualityAreaService", "No transformation required, returning data as is.");
      return data;
    }

    public Task<IDataResponse<IDataModel>> GetFromServer(string id, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public async Task<IDataResponse<IEnumerable<IDataModel>>> GetAllFromServer(CancellationToken cancellationToken)
    {
      var result = await HttpService.GetAll<QualityAreaModel>(Url, HttpMessageHandlerService.GetMessageHandler(), cancellationToken);

      return new DataResponse<IEnumerable<IDataModel>>
      {
        Data = result.Data.Cast<IDataModel>(),
        HttpResponseCode = result.HttpResponseCode
      };
    }

    public async Task<IDataResponse<IEnumerable<IDataModel>>> GetAllFromServerByKey(string key, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task<IDataResponse<IDataModel>> CreateOnServer(IDataModel model, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task<IDataResponse<IDataModel>> UpdateOnServer(IDataModel model, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task<IDataResponse<bool>> DeleteFromServer(string key, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public T GetLocal<T>(string key) where T : IDataModel
    {
      return CacheService.GetLocal<T>(key);
    }

    public int InsertLocal<T>(T data) where T : IDataModel
    {
      CacheService.InsertOrReplace<T>(data.Key, data);

      return 1;
    }

    public int UpdateLocal<T>(T data) where T : IDataModel
    {
      CacheService.InsertOrReplace<T>(data.Key, data);

      return 1;
    }

    public int InsertAllLocal<T>(IEnumerable<T> data) where T : IDataModel
    {
      CacheService.InsertOrReplace<T>("x", data);

      return 1;
    }

    public int DeleteLocal<T>(string key) where T : IDataModel
    {
      CacheService.RemoveIfExists(key);

      return 1;
    }

    public int DeleteAllLocal<T>() where T : IDataModel
    {
      return 1;
    }

    #endregion

    #region ILocalDbService

    //public IDataModel GetLocal(string key)
    //{
    //  throw new NotImplementedException();
    //}

    //public int InsertAll(IEnumerable<IDataModel> data)
    //{
    //  // mock response for now...
    //  Logger.Info("QualityAreaService", $"Inserting {data.Count()} item(s) into local DB.");
    //  return data.Count();
    //}

    //public int DeleteAll<T>()
    //{
    //  // mock response for now...
    //  Logger.Info("QualityAreaService", $"Deleting all from local DB.");
    //  return 3;
    //}

    //public int Delete<IDataModel>(string key)
    //{
    //  throw new NotImplementedException();
    //}

    //public int Insert(IDataModel data)
    //{
    //  throw new NotImplementedException();
    //}

    //public int Update(IDataModel data)
    //{
    //  throw new NotImplementedException();
    //}

    #endregion
  }
}
