using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Models.Interfaces;
using DataIntegrityService.Core.Services.ChangeTracking.Interfaces;
using DataIntegrityService.Core.Services.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

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
    public async Task Initialise()
    {
      // fetch tracked changes from server...
      var serverResponse = await GetAllTrackedChanges();

      if (serverResponse != null && serverResponse.ActionCancelled)
      {
        TrackedChanges = (List<DataChangeTrackingModel>)serverResponse.Data;
      }

      IsInitialised = true;
      await Task.CompletedTask;
    }

    public Task CompressPendingChanges()
    {
      // create a dictionary to store the latest action for each artifact...
      var pendingActions = new Dictionary<string, DataChangeTrackingModel>();

      // iterate through each message in the queue...
      foreach(var message in TrackedChanges)
      {
        // is the artifact in the dictionary?
        if (pendingActions.ContainsKey(message.DatasetName))
        {
          // if the current action is "Delete", remove the artifact from the dictionary...
          if (message.Action == "Delete")
            pendingActions.Remove(message.DatasetName);
          else // Update the action for the artifact...
            pendingActions[message.DatasetName] = message;
        }
        else // if the artifact is not in the dictionary, add it with the current action...
          pendingActions.Add(message.DatasetName, message);
      }

      TrackedChanges.Clear();
      TrackedChanges = pendingActions.Values.ToList(); // todo: order by dependency?

      return Task.CompletedTask;
    }

    public bool ChangesExist()
    {
      return TrackedChanges != null && TrackedChanges.Count > 0;
    }

    public async Task<IDataResponse<IEnumerable<IDataModel>>> GetAllTrackedChanges()
    {
      var result = await HttpService.GetAll<DataChangeTrackingModel>($"api/v1/ChangeTracking/GetChangeTrackingByUser/{HttpService.User.UserId}", HttpMessageHandlerService.GetMessageHandler(), CancellationToken);

      // ensure we are ordered by Utc date...
      if (result != null || result!.ActionSucceeded)
      {
        result.Data = result.Data.OrderBy(x => x.Created).ToList();
      }

      return new DataResponse<IEnumerable<IDataModel>>
      {
        Data = result.Data.Cast<IDataModel>(),
        HttpResponseCode = result.HttpResponseCode
      };
    }

    public async Task FlagAsCompleted(DataChangeTrackingModel item)
    {
      await HttpService.Delete($"api/v1/ChangeTracking/FlagAsCompleted/{item.Id}", HttpMessageHandlerService.GetMessageHandler());
    }

    public async Task FlagAsPoison(DataChangeTrackingModel item)
    {
      await HttpService.Get($"api/v1/ChangeTracking/FlagAsPoison/{item.Id}", HttpMessageHandlerService.GetMessageHandler(), CancellationToken);
    }

    public async Task FlushAllPendingChanges()
    {
      await HttpService.Delete($"api/v1/ChangeTracking/FlushAll", HttpMessageHandlerService.GetMessageHandler());
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
