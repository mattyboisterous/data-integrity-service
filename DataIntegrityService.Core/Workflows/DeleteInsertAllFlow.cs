﻿using DataIntegrityService.Core.Interfaces;
using DataIntegrityService.Core.Logging;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Services.Http;
using DataIntegrityService.Core.Services.Local;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Workflows
{
  public class DeleteInsertAllFlow : IWorkflowService
  {
    public string Key => "DeleteInsertAll";

    public async Task Execute(IDataService dataService, HttpMessageHandler messageHandler, CancellationTokenSource cancellationTokenSource)
    {
      try
      {
        Logger.Info("DeleteInsertAllFlow", "DeleteInsertAllFlow running...");

        if (dataService.IsInitialised)
        {
          Logger.Info("DeleteInsertAllFlow", $"Data service '{dataService.Key}' initialised, fetching data from backend Api...");

          // fetch all data from the server...
          var dataResponse = await dataService.GetAllFromServer(messageHandler, cancellationTokenSource);

          if (dataResponse != null && dataResponse.MethodSucceeded)
          {
            Logger.Info("DeleteInsertAllFlow", "Data received, looking to perform any necessary transformations...");

            // perform any data tranformation before attempting to store data locally...
            var data = dataService.TransformData(dataResponse.Data);

            // delete and insert all in cache, if configured...
            if (dataService is ILocalCacheService)
            {
              Logger.Info("DeleteInsertAllFlow", $"Data service '{dataService.Key}' uses a local cache service, removing and inserting all data...");

              ((ILocalCacheService)dataService).RemoveIfExists(string.Format(dataService.Settings.Cache.Key)); 

              ((ILocalCacheService)dataService).Insert(string.Format(dataService.Settings.Cache.Key), data);
            }

            // delete and insert all in Db, if configured...
            if (dataService is ILocalDbService)
            {
              Logger.Info("DeleteInsertAllFlow", $"Data service '{dataService.Key}' uses a local DB service, removing and inserting all data...");

              ((ILocalDbService)dataService).DeleteAll<IDataModel>();

              ((ILocalDbService)dataService).InsertAll(data);
            }
          }
          else
            Logger.Error("DeleteInsertAllFlow", "Api error, no  data returned.");

          Logger.Info("DeleteInsertAllFlow", "DeleteInsertAllFlow completed.");

          //CurrentRunState.BytesDownloaded += Utilities.GetObjectSize(qas);
          //dataSet.Count = qas.Count;
        }
      }
      catch (Exception ex)
      {
        Logger.Error("DeleteInsertAllFlow", ex.ToString());
      }
    }
  }
}
