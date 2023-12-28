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
          if (message.Action == ChangeAction.Delete.ToString())
          {
            Logger.Info("PullFromServer", $"Deleting data from back end...");
            return await dataService.DeleteFromServer(message.Key, cancellationToken);
          }
          else
          {
            Logger.Info("PullFromServer", $"Data service '{dataService.Key}' initialised, fetching data from local device...");

            IDataModel? dataModel = null;

            // fetch model from local store...
            if (dataService is ILocalCacheService)
            {
              Logger.Info("PullFromServer", $"Data service '{dataService.Key}' uses a local cache service, fetching data...");

              dataModel = ((ILocalCacheService)dataService).GetLocal(message.Key);
            }
            if (dataService is ILocalDbService)
            {
              Logger.Info("PullFromServer", $"Data service '{dataService.Key}' uses a local DB service, fetching data...");

              dataModel = ((ILocalDbService)dataService).GetLocal(message.Key);
            }

            if (dataModel != null)
            {
              Logger.Info("PullFromServer", "Data received, looking to perform any necessary transformations...");

              // perform any data tranformation before attempting to push data to server...
              var data = dataService.TransformData(dataModel);

              if (message.Action == ChangeAction.Create.ToString())
              {
                Logger.Info("PullFromServer", $"Creating data in back end...");
                return await dataService.CreateOnServer(data, cancellationToken);
              }
              else
              {
                Logger.Info("PullFromServer", $"Updating data in back end...");
                return await dataService.UpdateOnServer(data, cancellationToken);
              }
            }
            else
            {
              Logger.Error("PullFromServer", $"Local data model '{message.DatasetName}' not found with key {message.Key}. Unable to push to server.");
              return new ActionResponse() { ActionSucceeded = false };
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
