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
  public class StaticChangeTrackingService : IStaticDataService
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

    public Task<IDataResponse<IEnumerable<StaticDataChangeTrackingModel>>> GetAllFromServer(CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
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
