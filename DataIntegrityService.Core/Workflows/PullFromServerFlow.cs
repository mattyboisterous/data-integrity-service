using DataIntegrityService.Core.Logging;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Models.Interfaces;
using DataIntegrityService.Core.Services.Interfaces;
using DataIntegrityService.Core.Services.Local;
using DataIntegrityService.Core.Workflows.Interfaces;

namespace DataIntegrityService.Core.Workflows
{
  public class PullFromServerFlow : IWorkflowService
  {
    public string Key => "PullFromServer";

    public async Task<IActionResponse> Execute(DataChangeTrackingModel message, IDataService dataService, CancellationToken cancellationToken)
    {
      try
      {
        Logger.Info("PullFromServer", "PullFromServer running...");

        if (dataService.IsInitialised)
        {
          Logger.Info("PullFromServer", $"Data service '{dataService.Key}' initialised, determining action to perform...");

          if (message.Action == ChangeAction.Delete.ToString())
          {
            Logger.Info("PullFromServer", $"Deleting data from local store...");

            if (dataService is ILocalCacheService)
            {
              Logger.Info("PullFromServer", $"Data service '{dataService.Key}' uses a local cache service, deleting data...");

              ((ILocalCacheService)dataService).RemoveIfExists(message.Key);
            }
            if (dataService is ILocalDbService)
            {
              Logger.Info("PullFromServer", $"Data service '{dataService.Key}' uses a local DB service, deleting data...");

              ((ILocalDbService)dataService).Delete<IDataModel>(message.Key);
            }

            Logger.Error("PullFromServer", $"Work complete.");
            return new ActionResponse();
          }
          else
          {
            // fetch model from server...
            var dataResponse = await dataService.GetFromServer(message.Key, cancellationToken);

            if (dataResponse.ActionSucceeded && dataResponse.Data != null)
            {
              Logger.Info("PullFromServer", "Data received, looking to perform any necessary transformations...");

              // perform any data tranformation before attempting to push data to local store...
              var data = dataService.TransformData(dataResponse.Data);

              if (message.Action == ChangeAction.Create.ToString())
              {
                if (dataService is ILocalCacheService)
                {
                  Logger.Info("PullFromServer", $"Data service '{dataService.Key}' uses a local cache service, inserting data...");

                  ((ILocalCacheService)dataService).InsertOrReplace(message.Key, data);
                }
                if (dataService is ILocalDbService)
                {
                  Logger.Info("PullFromServer", $"Data service '{dataService.Key}' uses a local DB service, inserting data...");

                  ((ILocalDbService)dataService).Insert(data);
                }
              }
              else
              {
                if (dataService is ILocalCacheService)
                {
                  Logger.Info("PullFromServer", $"Data service '{dataService.Key}' uses a local cache service, updating data...");

                  ((ILocalCacheService)dataService).InsertOrReplace(message.Key, data);
                }
                if (dataService is ILocalDbService)
                {
                  Logger.Info("PullFromServer", $"Data service '{dataService.Key}' uses a local DB service, updating data...");

                  ((ILocalDbService)dataService).Update(data);
                }
              }

              Logger.Error("PullFromServer", $"Work complete.");
              return new ActionResponse();
            }
            else 
            {
              Logger.Error("PullFromServer", $"Server data model '{message.DatasetName}' not found with key {message.Key}. Unable to update locally.");
              return dataResponse;
            }
          }

          //CurrentRunState.BytesDownloaded += Utilities.GetObjectSize(qas);
          //dataSet.Count = qas.Count;
        }
        else
          throw new InvalidOperationException("Please initialise 'PullFromServer' before calling 'Execute'.");
      }
      catch (Exception ex)
      {
        Logger.Error("PullFromServer", ex.ToString());
        throw;
      }
    }
  }
}
