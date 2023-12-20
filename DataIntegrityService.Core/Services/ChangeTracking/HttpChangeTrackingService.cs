using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Services.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Services.ChangeTracking
{
  public class HttpChangeTrackingService : IChangeTrackingService
  {
    private IHttpService HttpService { get; set; }
    private IHttpMessageHandlerService HttpMessageHandlerService { get; set; }
    public CancellationToken CancellationToken { get; set; }

    public string Key => "HttpChangeTrackingService";
    public bool IsInitialised { get; set; }
    public string UserId { get; set; }

    public List<DataChangeTrackingModel> TrackedChanges { get; set; }

    public HttpChangeTrackingService(
      IHttpService httpService,
      IHttpMessageHandlerService messageHandlerService)
    {
      HttpService = httpService;
      HttpMessageHandlerService = messageHandlerService;
    }

    /// <summary>
    /// Get a collection of pending changes for this user from the server.
    /// </summary>
    public async void Initialise()
    {
      // fetch tracked changes from server...
      var serverResponse = await GetAllTrackedChanges();

      if (serverResponse != null && serverResponse.ActionSucceeded)
      {
        TrackedChanges = (List<DataChangeTrackingModel>)serverResponse.Data;
      }

      IsInitialised = true;
    }

    public bool ChangesExist()
    {
      return TrackedChanges != null && TrackedChanges.Count > 0;
    }

    public async Task<IDataResponse<IEnumerable<IDataModel>>> GetAllTrackedChanges()
    {
      var result = await HttpService.GetAll<DataChangeTrackingModel>($"api/v1/ChangeTracking/GetChangeTrackingByUser/{HttpService.User.UserId}", HttpMessageHandlerService.GetMessageHandler(), CancellationToken);

      return new DataResponse<IEnumerable<IDataModel>>
      {
        Data = result.Data.Cast<IDataModel>(),
        ActionSucceeded = result.ActionSucceeded
      };
    }

    public async Task FlagAsCompleted(DataChangeTrackingModel item)
    {
      await HttpService.Delete<int>($"api/v1/ChangeTracking/FlagAsCompleted/{item.Id}", HttpMessageHandlerService.GetMessageHandler());
    }

    public async Task FlagAsPoison(DataChangeTrackingModel item)
    {
      await HttpService.Get($"api/v1/ChangeTracking/FlagAsPoison/{item.Id}", HttpMessageHandlerService.GetMessageHandler(), CancellationToken);
    }

    public async Task FlushAllPendingChanges()
    {
      await HttpService.Delete<int>($"api/v1/ChangeTracking/FlushAll", HttpMessageHandlerService.GetMessageHandler());
    }

    public DataChangeTrackingModel GetNextChange()
    {
      //var response = await HttpService.Get<DataChangeTrackingModel>($"api/v1/ChangeTracking/Get/{item.Id}", HttpMessageHandlerService.GetMessageHandler(), CancellationToken);
      return null;
    }

    public Task IncrementAttempt(DataChangeTrackingModel item)
    {
      throw new NotImplementedException();
    }
  }
}
