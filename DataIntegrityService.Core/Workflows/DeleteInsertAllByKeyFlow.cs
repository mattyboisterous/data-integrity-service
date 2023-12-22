using DataIntegrityService.Core.Logging;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Models.Interfaces;
using DataIntegrityService.Core.Services.Http;
using DataIntegrityService.Core.Services.Interfaces;
using DataIntegrityService.Core.Services.Local;
using DataIntegrityService.Core.Workflows.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Workflows
{
    public class DeleteInsertAllByKeyFlow : IWorkflowService
  {
    public string Key => "DeleteInsertAllByKey";

    public async Task<IActionResponse> Execute(DataChangeTrackingModel message, IDataService dataService, CancellationToken cancellationToken)
    {
      try
      {
        if (dataService.IsInitialised)
        {
          Logger.Info("DeleteInsertAllByKeyFlow", $"Data service '{dataService.Key}' initialised, fetching data from backend Api...");

          // fetch all data from the server...
          var dataResponseAllKeys = await dataService.GetAllFromServer(cancellationToken);

          if (dataResponseAllKeys != null && dataResponseAllKeys.ActionSucceeded)
          {
            Logger.Info("DeleteInsertAllByKey", "Data received, about to iterate and fetch data by key...");

            foreach (var item in dataResponseAllKeys.Data)
            {
              // fetch all data using the item key...
              var dataResponseByKey = await dataService.GetAllFromServerByKey(item.Key, cancellationToken);

              if (dataResponseByKey != null && dataResponseByKey.ActionSucceeded)
              {
                Logger.Info("DeleteInsertAllByKey", "Data received, looking to perform any necessary transformations...");

                // perform any data tranformation before attempting to store data locally...
                var data = dataService.TransformData(dataResponseByKey.Data);

                // delete and insert all in cache, if configured...
                if (dataService is ILocalCacheService)
                {
                  Logger.Info("DeleteInsertAllByKey", $"Data service '{dataService.Key}' uses a local cache service, removing and inserting all data...");

                  ((ILocalCacheService)dataService).RemoveIfExists(string.Format(dataService.Settings.Cache.KeyMap, item.Key));

                  ((ILocalCacheService)dataService).InsertOrReplace(string.Format(dataService.Settings.Cache.KeyMap, item.Key), data);
                }

                // delete and insert all in Db, if configured...
                if (dataService is ILocalDbService)
                {
                  Logger.Info("DeleteInsertAllByKey", $"Data service '{dataService.Key}' uses a local DB service, removing and inserting all data...");

                  ((ILocalDbService)dataService).DeleteAll<IDataModel>();

                  ((ILocalDbService)dataService).InsertAll(data);
                }
              }
              else
              {
                Logger.Error("DeleteInsertAllByKey", "Api error, no data returned.");
                return dataResponseByKey!;
              }
            }
          }
          else
          {
            Logger.Error("DeleteInsertAllByKey", "Api error, no  data returned.");
            return dataResponseAllKeys!;
          }

          Logger.Info("DeleteInsertAllByKey", "DeleteInsertAllByKey completed.");
          return new ActionResponse();

          //CurrentRunState.BytesDownloaded += Utilities.GetObjectSize(qas);
          //dataSet.Count = qas.Count;
        }
        else
          throw new InvalidOperationException("Please initialise 'DeleteInsertAllByKey' before calling 'Execute'.");
      }
      catch (Exception ex)
      {
        Logger.Error("DeleteInsertAllByKey", ex.ToString());
        throw;
      }
    }
  }
}
