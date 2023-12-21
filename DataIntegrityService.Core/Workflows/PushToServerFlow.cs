using DataIntegrityService.Core.Interfaces;
using DataIntegrityService.Core.Logging;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Services.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataIntegrityService.Core.Workflows
{
  public class PushToServerFlow : IWorkflowService
  {
    public string Key => "PushToServer";

    public async Task<IActionResponse> Execute(DataChangeTrackingModel message, IDataService dataService, CancellationToken cancellationToken)
    {
      try
      {
        Logger.Info("PushToServer", "PushCreate running...");

        if (dataService.IsInitialised)
        {
          if (message.Action == ChangeAction.Delete.ToString())
          {
            Logger.Info("PushToServer", $"Deleting data from back end...");
            return await dataService.DeleteFromServer(message.Key, cancellationToken);
          }
          else
          {
            Logger.Info("PushToServer", $"Data service '{dataService.Key}' initialised, fetching data from local device...");

            IDataModel? dataModel = null;

            // fetch model from local store...
            if (dataService is ILocalCacheService)
            {
              Logger.Info("PushToServer", $"Data service '{dataService.Key}' uses a local cache service, fetching data...");

              dataModel = ((ILocalCacheService)dataService).GetLocal(message.Key);
            }
            if (dataService is ILocalDbService)
            {
              Logger.Info("PushToServer", $"Data service '{dataService.Key}' uses a local DB service, fetching data...");

              dataModel = ((ILocalDbService)dataService).GetLocal(message.Key);
            }

            if (dataModel != null)
            {
              Logger.Info("PushToServer", "Data received, looking to perform any necessary transformations...");

              // perform any data tranformation before attempting to push data to server...
              var data = dataService.TransformData(dataModel);

              if (message.Action == ChangeAction.Create.ToString())
              {
                Logger.Info("PushToServer", $"Creating data in back end...");
                return await dataService.CreateOnServer(data, cancellationToken);
              }
              else
              {
                Logger.Info("PushToServer", $"Updating data in back end...");
                return await dataService.UpdateOnServer(data, cancellationToken);
              }
            }
            else
            {
              Logger.Error("PushToServer", $"Local data model '{message.DatasetName}' not found with key {message.Key}. Unable to push to server.");
              return new ActionResponse() { ActionSucceeded = false };
            }
          }

          //CurrentRunState.BytesDownloaded += Utilities.GetObjectSize(qas);
          //dataSet.Count = qas.Count;
        }
        else
          throw new InvalidOperationException("Please initialise 'PushToServer' before calling 'Execute'.");
      }
      catch (Exception ex)
      {
        Logger.Error("PushToServer", ex.ToString());
        throw;
      }
    }
  }
}
