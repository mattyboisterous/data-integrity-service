using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Models.Interfaces;
using DataIntegrityService.Core.Services.Http;
using DataIntegrityService.Core.Services.Interfaces;
using DataIntegrityService.Core.Services.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Services
{
  public class StaticChangeTrackingService : IDataService, IStaticDataService
  {
    private IHttpService HttpService { get; set; }
    private IHttpMessageHandlerService HttpMessageHandlerService { get; set; }
    private ILocalCacheService CacheService { get; set; }
    public CancellationToken CancellationToken { get; set; }
    public string Key => "StaticChangeTracking";
    public bool IsInitialised { get; set; }
    public List<StaticDataChangeTrackingModel> LocalReferenceDataSetState { get; set; } = new List<StaticDataChangeTrackingModel>();  
    public List<StaticDataChangeTrackingModel> ServerReferenceDataSetState { get; set; } = new List<StaticDataChangeTrackingModel>(); 
    public DataServiceConfiguration Settings { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public StaticChangeTrackingService(
      IHttpService httpService,
      IHttpMessageHandlerService httpMessageHandlerService,
      ILocalCacheService cacheService)
    {
      HttpService = httpService;
      HttpMessageHandlerService = httpMessageHandlerService;
      CacheService = cacheService;
    }

    public async Task Initialise()
    {
      // fetch tracked local changes...
      LocalReferenceDataSetState = CacheService.GetAllLocal<StaticDataChangeTrackingModel>(Key).ToList();

      // fetch tracked changes from server...
      var serverResponse = await GetAllFromServer(CancellationToken);

      if (serverResponse != null && serverResponse.ActionSucceeded)
      {
        ServerReferenceDataSetState = (List<StaticDataChangeTrackingModel>)serverResponse.Data;
      }

      IsInitialised = true;
      await Task.CompletedTask;
    }

    #region IDataService Members

    public IDataModel TransformData(IDataModel data)
    {
      return data;
    }

    public IEnumerable<IDataModel> TransformData(IEnumerable<IDataModel> data)
    {
      return data;
    }

    public Task<IDataResponse<IDataModel>> CreateOnServer(IDataModel model, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<IDataResponse<IDataModel>> UpdateOnServer(IDataModel model, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<IDataResponse<bool>> DeleteFromServer(string key, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<IDataResponse<IDataModel>> GetFromServer(string id, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<Models.Interfaces.IDataResponse<IEnumerable<IDataModel>>> GetAllFromServer(CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task<IDataResponse<IEnumerable<IDataModel>>> GetAllFromServerByKey(string key, CancellationToken cancellationToken) => throw new NotImplementedException();

    public T GetLocal<T>(string key) where T : IDataModel
    {
      throw new NotImplementedException();
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
  }
}
