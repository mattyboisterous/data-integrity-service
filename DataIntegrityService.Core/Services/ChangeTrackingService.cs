using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Services.Http;
using DataIntegrityService.Core.Services.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Services
{
  public class ChangeTrackingService: ILocalCacheService
  {
    private IHttpService HttpService { get; set; }
    public string Key => "ChangeTracking";
    public bool IsInitialised { get; set; }
    public string UserId { get; set; }
    public List<AllModelDataSetChange> TrackedChangesLocal { get; set; }
    public List<AllModelDataSetChange> TrackedChangesServer { get; set; }

    public ChangeTrackingService(IHttpService httpService)
    {
      HttpService = httpService;
    }

    public async Task Initialise()
    {
      // fetch tracked local changes...
      TrackedChangesLocal = (List<AllModelDataSetChange>)GetAllChangesFromLocalCache();

      // fetch tracked changes from server...
      var serverResponse = await GetAllChangesFromServer(UserId, null, null);

      if (serverResponse != null && serverResponse.MethodSucceeded)
      {
        TrackedChangesServer = (List<AllModelDataSetChange>)serverResponse.Data;
      }

      IsInitialised = true;
    }

    public IEnumerable<IDataModel> GetAllChangesFromLocalCache()
    {
      return GetAllLocal("localKey");
    }

    public async Task<IDataResponse<IEnumerable<IDataModel>>> GetAllChangesFromServer(string userId, HttpMessageHandler messageHandler, CancellationTokenSource cancellationTokenSource)
    {
      var result = await HttpService.GetAll<AllModelDataSetChange>($"api/v1/GetChangeTrackingByUser/{UserId}", messageHandler, cancellationTokenSource);

      return new DataResponse<IEnumerable<IDataModel>>
      {
        Data = result.Data.Cast<IDataModel>(),
        MethodSucceeded = result.MethodSucceeded
      };
    }

    public IEnumerable<IDataModel> GetAllLocal(string key)
    {
      throw new NotImplementedException();
    }

    public void RemoveIfExists(string key)
    {
      throw new NotImplementedException();
    }

    public void InsertOrReplace(string key, IEnumerable<IDataModel> data)
    {
      throw new NotImplementedException();
    }
  }
}
