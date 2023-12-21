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
  public class PushCreateFlow : IWorkflowService
  {
    public string Key => "PushCreate";

    public async Task<IActionResponse> Execute(DataChangeTrackingModel message, IDataService dataService, CancellationToken cancellationToken)
    {
      try
      {
        Logger.Info("PushCreate", "PushCreate running...");

        if (dataService.IsInitialised)
        {
          Logger.Info("PushCreate", $"Data service '{dataService.Key}' initialised, fetching data from local device...");

          IDataModel? dataModel = null;

          // fetch model from local store...
          if (dataService is ILocalCacheService)
          {
            Logger.Info("PushCreate", $"Data service '{dataService.Key}' uses a local cache service, fetching data...");

            dataModel = ((ILocalCacheService)dataService).GetLocal(message.Key);
          }
          if (dataService is ILocalDbService)
          {
            Logger.Info("PushCreate", $"Data service '{dataService.Key}' uses a local DB service, fetching data...");

            dataModel = ((ILocalDbService)dataService).GetLocal(message.Key);
          }

          if (dataModel != null)
          {
            Logger.Info("PushCreate", "Data received, looking to perform any necessary transformations...");

            // perform any data tranformation before attempting to store data locally...
            var data = dataService.TransformData(dataModel);

            Logger.Info("PushCreate", $"Creating data in back end...");
            return await dataService.PushToServer(data, cancellationToken);
          }
          else
          {
            Logger.Error("PushCreate", $"Local data model '{message.DatasetName}' not found with key {message.Key}. Unable to push to server.");
            return new ActionResponse() { ActionSucceeded = false };
          }

          //CurrentRunState.BytesDownloaded += Utilities.GetObjectSize(qas);
          //dataSet.Count = qas.Count;
        }
        else
          throw new InvalidOperationException("Please initialise 'PushCreate' before calling 'Execute'.");
      }
      catch (Exception ex)
      {
        Logger.Error("PushCreate", ex.ToString());
        throw;
      }
    }
  }
}
