using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Models.Interfaces;
using DataIntegrityService.Core.Services.ChangeTracking.Interfaces;
using DataIntegrityService.Core.Services.Http;
using DataIntegrityService.Core.Services.Local;
using System;

namespace DataIntegrityService.Core.Services.ChangeTracking.Static
{
  public class StaticChangeTrackingService : IStaticChangeTrackingService
  {
    private IHttpService HttpService { get; set; }
    private IHttpMessageHandlerService HttpMessageHandlerService { get; set; }
    private ILocalCacheService CacheService { get; set; }

    public CancellationToken CancellationToken { get; set; }

    public string Key => "StaticChangeTracking";
    public bool IsInitialised { get; set; }

    public List<StaticDataChangeTrackingModel> LocalReferenceDataSetState { get; set; } = new List<StaticDataChangeTrackingModel>();
    public List<StaticDataChangeTrackingModel> ServerReferenceDataSetState { get; set; } = new List<StaticDataChangeTrackingModel>();
    public required StaticChangeTrackingConfiguration Settings { get; set; }
    public bool ForceRehydrateAll { get; set; }

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

    public async Task<IDataResponse<IEnumerable<StaticDataChangeTrackingModel>>> GetAllFromServer(CancellationToken cancellationToken)
    {
      var result = await HttpService.GetAll<StaticDataChangeTrackingModel>(Settings.Http.GetAll, HttpMessageHandlerService.GetMessageHandler(), cancellationToken);

      return new DataResponse<IEnumerable<StaticDataChangeTrackingModel>>
      {
        Data = result.Data,
        HttpResponseCode = result.HttpResponseCode
      };
    }

    public IEnumerable<StaticDataChangeTrackingModel> GetAllLocal(string key)
    {
      throw new NotImplementedException();
    }

    public int UpsertLocal(string key, IEnumerable<StaticDataChangeTrackingModel> items)
    {
      throw new NotImplementedException();
    }
  }
}
