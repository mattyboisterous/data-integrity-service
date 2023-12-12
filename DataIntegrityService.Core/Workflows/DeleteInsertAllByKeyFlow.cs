//using DataIntegrityService.Core.Interfaces;
//using DataIntegrityService.Core.Models;
//using DataIntegrityService.Core.Services.Http;
//using DataIntegrityService.Core.Services.Local;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DataIntegrityService.Core.Workflows
//{
//  public class DeleteInsertAllByKeyFlow : IWorkflowService
//  {
//    public string Key => "DeleteInsertAllByKey";

//    public async Task Execute(IDataService dataService, IDataModel sourceType, IDataModel destinationType, HttpMessageHandler messageHandler, CancellationTokenSource cancellationTokenSource)
//    {
//      try
//      {
//        if (dataService.IsInitialised)
//        {
//          // get all keys...
//          var dataResponseAllKeys = await ((IHttpGetService)dataService).HttpGetAll<IDataModel>(((IHttpGetService)dataService).Url, messageHandler, cancellationTokenSource);

//          if (dataResponseAllKeys != null && dataResponseAllKeys.MethodSucceeded)
//          {
//            foreach(var item in dataResponseAllKeys.Data)
//            {
//              // parse the url map at this point using the key...
//              var dataResponse = await ((IHttpGetService)dataService).HttpGetAll<IDataModel>(((IHttpGetService)dataService).Url, messageHandler, cancellationTokenSource);
//            }

//            // todo: now just IDataModel to IDataModel?



//            // perform any model mapping before attempting to store data locally...
//            var data = modelMapper.MapDataModels(dataResponse.Data);

//            // delete and insert all in cache, if configured...
//            if (dataService is ILocalCacheService)
//            {
//              ((ILocalCacheService)dataService).RemoveIfExists<T2>(((ILocalCacheService)dataService).CacheKeyMap); // todo: this is the map, need to determine final key, same with Url...

//              ((ILocalCacheService)dataService).Insert(((ILocalCacheService)dataService).CacheKeyMap, dataResponse.Data);
//            }

//            // delete and insert all in Db, if configured...
//            if (dataService is ILocalDbService)
//            {
//              ((ILocalDbService)dataService).DeleteAll<T2>();

//              ((ILocalDbService)dataService).InsertAll(dataResponse.Data);
//            }
//          }

//          //CurrentRunState.BytesDownloaded += Utilities.GetObjectSize(qas);
//          //dataSet.Count = qas.Count;
//        }
//      }
//      catch (Exception ex)
//      {
//      }
//    }
//  }
//}
