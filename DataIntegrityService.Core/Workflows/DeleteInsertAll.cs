using DataIntegrityService.Core.Interfaces;
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
  public class DeleteInsertAll
  {
    public async void Execute<T>(IDataService dataService, T modelType, HttpMessageHandler messageHandler, CancellationTokenSource cancellationTokenSource)
    {

      // todo: Mapper to translate Http models to local models possibly?

      try
      {
        if (dataService is IHttpGetService)
        {
          // todo: need to format url mask...

          var dataResponse = await ((IHttpGetService)dataService).HttpGetAll<T>(dataService.Settings.Http.Get, messageHandler, cancellationTokenSource);

          if (dataResponse != null && dataResponse.MethodSucceeded)
          {
            // delete and insert all in cache, if configured...
            if (dataService is ILocalCacheService)
            {
              ((ILocalCacheService)dataService).RemoveIfExists<T>(((ILocalCacheService)dataService).CacheKeyMap); // todo: this is the map, need to determine final key, same with Url...

              ((ILocalCacheService)dataService).Insert(((ILocalCacheService)dataService).CacheKeyMap, dataResponse.Data);
            }

            // delete and insert all in Db, if configured...
            if (dataService is ILocalDbService)
            {
              ((ILocalDbService)dataService).DeleteAll<T>();

              ((ILocalDbService)dataService).InsertAll<T>(dataResponse.Data);
            }
          }
        }

        //CurrentRunState.BytesDownloaded += Utilities.GetObjectSize(qas);
        //dataSet.Count = qas.Count;
      }
      catch (Exception ex)
      { 
      }
    }
  }
}
