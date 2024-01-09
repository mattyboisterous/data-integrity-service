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
  public class StaticChangeTrackingService : IDataService, IStaticDataService, ILocalCacheService
  {
    private IHttpService HttpService { get; set; }
    private IHttpMessageHandlerService HttpMessageHandlerService { get; set; }
    public CancellationToken CancellationToken { get; set; }
    public string Key => "StaticChangeTracking";
    public bool IsInitialised { get; set; }
    public List<StaticDataChangeTrackingModel> LocalReferenceDataSetState { get; set; } = new List<StaticDataChangeTrackingModel>();  
    public List<StaticDataChangeTrackingModel> ServerReferenceDataSetState { get; set; } = new List<StaticDataChangeTrackingModel>(); 
    public DataServiceConfiguration Settings { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public StaticChangeTrackingService(
      IHttpService httpService,
      IHttpMessageHandlerService httpMessageHandlerService)
    {
      HttpService = httpService;
      HttpMessageHandlerService = httpMessageHandlerService;
    }

    public async Task Initialise()
    {
      // fetch tracked local changes...
      LocalReferenceDataSetState = (List<StaticDataChangeTrackingModel>)GetAllLocal<StaticDataChangeTrackingModel>(Key);

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

    #endregion

    #region ILocalCacheService

    public T GetLocal<T>(string key) where T : IDataModel
    {
      throw new NotImplementedException();
    }

    public IEnumerable<T> GetAllLocal<T>(string key) where T : IDataModel
    {
      throw new NotImplementedException();
    }

    public void InsertOrReplace<T>(string key, T data) where T : IDataModel
    {
      throw new NotImplementedException();
    }

    public void InsertOrReplace<T>(string key, IEnumerable<T> data) where T : IDataModel
    {
      throw new NotImplementedException();
    }

    public void RemoveIfExists(string key)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
