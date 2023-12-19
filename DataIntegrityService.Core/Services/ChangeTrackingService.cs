//using DataIntegrityService.Core.Models;
//using DataIntegrityService.Core.Services.Http;
//using DataIntegrityService.Core.Services.Local;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DataIntegrityService.Core.Services
//{
//  public class ChangeTrackingService: ILocalCacheService
//  {
//    private IHttpService HttpService { get; set; }
//    public string Key => "ChangeTracking";
//    public bool IsInitialised { get; set; }
//    public string UserId { get; set; }
//    public List<DataChangeTrackingModel> TrackedChangesLocal { get; set; }
//    public List<DataChangeTrackingModel> TrackedChangesServer { get; set; }

//    public ChangeTrackingService(IHttpService httpService)
//    {
//      HttpService = httpService;
//    }

//    public async Task Initialise()
//    {
//      // fetch tracked local changes...
//      TrackedChangesLocal = (List<DataChangeTrackingModel>)GetAllChangesFromLocalCache();

//      // fetch tracked changes from server...
//      var serverResponse = await GetAllChangesFromServer(UserId, null, null);

//      if (serverResponse != null && serverResponse.ActionSucceeded)
//      {
//        TrackedChangesServer = (List<DataChangeTrackingModel>)serverResponse.Data;
//      }

//      IsInitialised = true;
//    }

//    public IEnumerable<IDataModel> GetAllChangesFromLocalCache()
//    {
//      return GetAllLocal("localKey");
//    }

//    public async Task<IDataResponse<IEnumerable<IDataModel>>> GetAllChangesFromServer(string userId, HttpMessageHandler messageHandler, CancellationToken cancellationToken)
//    {
//      var result = await HttpService.GetAll<DataChangeTrackingModel>($"api/v1/GetChangeTrackingByUser/{UserId}", messageHandler, cancellationToken);

//      return new DataResponse<IEnumerable<IDataModel>>
//      {
//        Data = result.Data.Cast<IDataModel>(),
//        ActionSucceeded = result.ActionSucceeded
//      };
//    }
    
//    public IEnumerable<IDataModel> GetAllLocal(string key)
//    {
//      throw new NotImplementedException();
//    }

//    public void RemoveIfExists(string key)
//    {
//      throw new NotImplementedException();
//    }

//    public void InsertOrReplace(string key, IEnumerable<IDataModel> data)
//    {
//      throw new NotImplementedException();
//    }
//  }
//}
