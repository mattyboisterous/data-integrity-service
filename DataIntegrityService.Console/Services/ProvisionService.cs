using DataIntegrityService.Console.Models;
using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Logging;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Models.Interfaces;
using DataIntegrityService.Core.Services.Http;
using DataIntegrityService.Core.Services.Interfaces;
using DataIntegrityService.Core.Services.Local;

namespace DataIntegrityService.Console.Providers
{
  public class ProvisionService : IDataService
  {
    private IHttpService HttpService { get; set; }
    private IHttpMessageHandlerService HttpMessageHandlerService { get; set; }
    private ILocalDbService DbService { get; set; }

    public string Key => "Provision";

    public required DataServiceConfiguration Settings { get; set; }

    public bool IsInitialised { get; set; }

    public required string Url { get; set; }

    public ProvisionService(
      IHttpService httpService,
      IHttpMessageHandlerService httpMessageHandlerService,
      ILocalDbService dbService)
    {
      HttpService = httpService;
      HttpMessageHandlerService = httpMessageHandlerService;
      DbService = dbService;
    }

    public async Task Initialise()
    {
      // do nothing...
      IsInitialised = true;
      await Task.CompletedTask;
    }

    #region IDataService members

    public IDataModel TransformData(IDataModel data)
    {
      // no transformations...
      return data;
    }

    public IEnumerable<IDataModel> TransformData(IEnumerable<IDataModel> data)
    {
      // no transformation required...
      Logger.Info("ProvisionService", "No transformation required, returning data.");
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
        ActionSucceeded = result.ActionSucceeded
      };
    }

    public async Task<IDataResponse<IEnumerable<IDataModel>>> GetAllFromServerByKey(string key, CancellationToken cancellationToken)
    {
      var result = await HttpService.GetAll<QualityAreaModel>(string.Format(Settings.Http.GetAllByKey, key), HttpMessageHandlerService.GetMessageHandler(), cancellationToken);

      return new DataResponse<IEnumerable<IDataModel>>
      {
        Data = result.Data.Cast<IDataModel>(),
        ActionSucceeded = result.ActionSucceeded
      };
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
      return DbService.GetLocal<T>(key);
    }

    public int InsertLocal<T>(T data) where T : IDataModel
    {
      throw new NotImplementedException();
    }

    public int UpdateLocal<T>(T data) where T : IDataModel
    {
      throw new NotImplementedException();
    }

    public int InsertAllLocal<T>(IEnumerable<T> data) where T : IDataModel
    {
      throw new NotImplementedException();
    }

    public int DeleteLocal<T>(string key) where T : IDataModel
    {
      throw new NotImplementedException();
    }

    public int DeleteAllLocal<T>() where T : IDataModel
    {
      throw new NotImplementedException();
    }

    #endregion

    //#region ILocalCacheService members

    //public IDataModel GetLocal(string key)
    //{
    //  throw new NotImplementedException();
    //}

    //public IEnumerable<IDataModel> GetAllLocal(string key)
    //{
    //  throw new NotImplementedException();
    //}

    //public void RemoveIfExists(string key)
    //{
    //  // mock response for now...
    //  Logger.Info("ProvisionService", $"Deleting all from local cache with key '{key}'.");
    //}

    //public void InsertOrReplace(string key, IEnumerable<IDataModel> data)
    //{
    //  // mock response for now...
    //  Logger.Info("ProvisionService", $"Inserting {data.Count()} item(s) into local cache with key '{key}'.");
    //}

    //public void InsertOrReplace(string key, IDataModel data)
    //{
    //  throw new NotImplementedException();
    //}

    //public T GetLocal<T>(string key) where T : IDataModel
    //{
    //  throw new NotImplementedException();
    //}

    //public IEnumerable<T> GetAllLocal<T>(string key) where T : IDataModel
    //{
    //  throw new NotImplementedException();
    //}

    //public void InsertOrReplace<T>(string key, T data) where T : IDataModel
    //{
    //  throw new NotImplementedException();
    //}

    //public void InsertOrReplace<T>(string key, IEnumerable<T> data) where T : IDataModel
    //{
    //  throw new NotImplementedException();
    //}

    //#endregion
  }
}
