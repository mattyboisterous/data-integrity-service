//using DataIntegrityService.Core.Configuration;
//using DataIntegrityService.Core.Interfaces;
//using DataIntegrityService.Core.Models;
//using DataIntegrityService.Core.Services.Http;
//using DataIntegrityService.Core.Services.Local;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection.Metadata.Ecma335;
//using System.Text;
//using System.Threading.Tasks;

//namespace DataIntegrityService.Core.Providers
//{
//  public class ProvisionService : IDataService, IHttpGetService, ILocalCacheService
//  {
//    public string Key => "Provision";

//    public required DataServiceConfiguration Settings { get; set; }

//    public string CacheKeyMap => "provision-{0}";

//    public bool IsInitialised { get; set; }

//    public required string Url { get; set; }

//    public void Initialise()
//    {
//      // do nothing...
//      IsInitialised = true;
//    }

//    #region IHttpGetService members

//    public Task<DataResponse<T>> HttpGet<T>(string url, HttpMessageHandler handler, CancellationTokenSource tokenSource = null)
//    {
//      throw new NotImplementedException();
//    }

//    public Task<DataResponse<List<T>>> HttpGetAll<T>(string url, HttpMessageHandler handler, CancellationTokenSource tokenSource = null)
//    {
//      throw new NotImplementedException();
//    }

//    #endregion

//    #region ILocalCacheService members

//    public List<T> GetAllLocal<T>()
//    {
//      throw new NotImplementedException();
//    }

//    public string GetCacheKey(string primaryKey)
//    {
//      return string.Format(CacheKeyMap, primaryKey);
//    }

//    public void Insert<T>(string key, T data)
//    {
//      throw new NotImplementedException();
//    }

//    public void RemoveIfExists<T>(string key)
//    {
//      throw new NotImplementedException();
//    }

//    #endregion
//  }
//}
