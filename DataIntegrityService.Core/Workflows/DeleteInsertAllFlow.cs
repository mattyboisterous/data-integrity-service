using DataIntegrityService.Core.Logging;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Models.Interfaces;
using DataIntegrityService.Core.Services.Http;
using DataIntegrityService.Core.Services.Interfaces;
using DataIntegrityService.Core.Services.Local;
using DataIntegrityService.Core.Workflows.Interfaces;
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

    public async Task<IActionResponse> ExecuteNonGeneric(DataChangeTrackingModel message, IDataService dataService, CancellationToken cancellationToken, string typeName)
    {
      Logger.Info("DeleteInsertAll", $"Resolving type '{typeName}'...");

      Type dataType = Type.GetType(typeName);

      // Call the generic method indirectly
      var method = typeof(DeleteInsertAllFlow).GetMethod("Execute").MakeGenericMethod(dataType);
      var resultTask = (Task<IActionResponse>)method.Invoke(this, new object[] { message, dataService, cancellationToken });

      return await resultTask;
    }

    public async Task<IActionResponse> Execute<T>(DataChangeTrackingModel message, IDataService dataService, CancellationToken cancellationToken) where T : IDataModel
    {
      try
      {
        Logger.Info("DeleteInsertAll", "DeleteInsertAllFlow running...");

        if (dataService.IsInitialised)
        {
          Logger.Info("DeleteInsertAll", $"Data service '{dataService.Key}' initialised, fetching data from backend Api...");

          // fetch all data from the server...
          var dataResponse = await dataService.GetAllFromServer(cancellationToken);

          if (dataResponse != null && dataResponse.ActionSucceeded)
          {
            Logger.Info("DeleteInsertAll", "Data received, looking to perform any necessary transformations...");

            // perform any data tranformation before attempting to store data locally...
            var data = dataService.TransformData(dataResponse.Data);

            // delete and insert all in cache, if configured...
            if (dataService is ILocalCacheService)
            {
              Logger.Info("DeleteInsertAll", $"Data service '{dataService.Key}' uses a local cache service, removing and inserting all data...");

              ((ILocalCacheService)dataService).RemoveIfExists(string.Format(dataService.Settings.Cache.Key)); 

              ((ILocalCacheService)dataService).InsertOrReplace(string.Format(dataService.Settings.Cache.Key), data);
            }

            // delete and insert all in Db, if configured...
            if (dataService is ILocalDbService)
            {
              Logger.Info("DeleteInsertAll", $"Data service '{dataService.Key}' uses a local DB service, removing and inserting all data...");

              ((ILocalDbService)dataService).DeleteAll<T>();

              ((ILocalDbService)dataService).InsertAll(data);
            }
          }
          else
          {
            Logger.Error("DeleteInsertAll", "Api error, no  data returned.");
            return dataResponse!;
          }

          Logger.Info("DeleteInsertAll", "DeleteInsertAllFlow completed.");
          return new ActionResponse();

          //CurrentRunState.BytesDownloaded += Utilities.GetObjectSize(qas);
          //dataSet.Count = qas.Count;
        }
        else
          throw new InvalidOperationException("Please initialise 'DeleteInsertAll' before calling 'Execute'.");
      }
      catch (Exception ex)
      {
        Logger.Error("DeleteInsertAll", ex.ToString());
        throw;
      }
    }
  }
}
