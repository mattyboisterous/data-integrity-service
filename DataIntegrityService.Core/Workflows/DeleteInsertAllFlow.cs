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
  public class DeleteInsertAllFlow : IWorkflowService
  {
    public string Key => "DeleteInsertAll";

    public async Task Execute<T1, T2>(IDataService dataService, T1 sourceType, T2 destinationType, HttpMessageHandler messageHandler, CancellationTokenSource cancellationTokenSource)
    {
      try
      {
        if (dataService.IsInitialised)
        {
          var dataResponse = await ((IHttpGetService)dataService).HttpGetAll<T1>(((IHttpGetService)dataService).Url, messageHandler, cancellationTokenSource);

          if (dataResponse != null && dataResponse.MethodSucceeded)
          {
            // perform any model mapping before attempting to store data locally...
            //var data = modelMapper.MapDataModels(dataResponse.Data);

            // delete and insert all in cache, if configured...
            if (dataService is ILocalCacheService)
            {
              ((ILocalCacheService)dataService).RemoveIfExists<IDataModel>(((ILocalCacheService)dataService).CacheKeyMap); // todo: this is the map, need to determine final key, same with Url...

              ((ILocalCacheService)dataService).Insert(((ILocalCacheService)dataService).CacheKeyMap, dataResponse.Data);
            }

            // delete and insert all in Db, if configured...
            if (dataService is ILocalDbService)
            {
              ((ILocalDbService)dataService).DeleteAll<IDataModel>();

              ((ILocalDbService)dataService).InsertAll(dataResponse.Data);
            }
          }

          //CurrentRunState.BytesDownloaded += Utilities.GetObjectSize(qas);
          //dataSet.Count = qas.Count;
        }
      }
      catch (Exception ex)
      {
      }
    }
  }
}
