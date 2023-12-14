using DataIntegrityService.Core.Interfaces;
using DataIntegrityService.Core.Logging;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Services.Http;
using DataIntegrityService.Core.Services.Local;
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

    public async Task Execute(IDataService dataService, HttpMessageHandler messageHandler, CancellationTokenSource cancellationTokenSource)
    {
      try
      {
        if (dataService.IsInitialised)
        {
          Logger.Info("DeleteInsertAllByKeyFlow", "Data service initialised, fetching data from backend Api...");

          // fetch all data from the server...
          var dataResponseAllKeys = await dataService.GetAllFromServer(messageHandler, cancellationTokenSource);

          if (dataResponseAllKeys != null && dataResponseAllKeys.MethodSucceeded)
          {
            Logger.Info("DeleteInsertAllByKey", "Data received, about to iterate and fetch data by key...");

            foreach (var item in dataResponseAllKeys.Data)
            {
              // fetch all data using the item key...
              var dataResponseByKey = await dataService.GetAllFromServerByKey(item.Key, messageHandler, cancellationTokenSource);

              if (dataResponseByKey != null && dataResponseByKey.MethodSucceeded)
              {
                Logger.Info("DeleteInsertAllByKey", "Data received, looking to perform any necessary transformations...");

                // perform any data tranformation before attempting to store data locally...
                var data = dataService.TransformData(dataResponseByKey.Data);

                // delete and insert all in cache, if configured...
                if (dataService is ILocalCacheService)
                {
                  Logger.Info("DeleteInsertAllByKey", "Data service uses a local cache service, removing and inserting all data...");

                  ((ILocalCacheService)dataService).RemoveIfExists(string.Format(dataService.Settings.Cache.KeyMap, item.Key));

                  ((ILocalCacheService)dataService).Insert(string.Format(dataService.Settings.Cache.KeyMap, item.Key), data);
                }

                // delete and insert all in Db, if configured...
                if (dataService is ILocalDbService)
                {
                  Logger.Info("DeleteInsertAllByKey", "Data service uses a local DB service, removing and inserting all data...");

                  ((ILocalDbService)dataService).DeleteAll<IDataModel>();

                  ((ILocalDbService)dataService).InsertAll(data);
                }
              }
            }
          }
          else
            Logger.Error("DeleteInsertAllByKey", "Api error, no  data returned.");

          Logger.Info("DeleteInsertAllByKey", "DeleteInsertAllByKey completed.");

          //CurrentRunState.BytesDownloaded += Utilities.GetObjectSize(qas);
          //dataSet.Count = qas.Count;
        }
      }
      catch (Exception ex)
      {
        Logger.Error("DeleteInsertAllByKey", ex.ToString());
      }
    }
  }
}
