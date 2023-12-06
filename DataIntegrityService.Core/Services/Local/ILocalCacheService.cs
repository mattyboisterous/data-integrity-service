using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Services.Local
{
  public interface ILocalCacheService
  {
    string CacheKeyMap { get; }
    string GetCacheKey(string primaryKey);
    List<T> GetAllLocal<T>();
    void RemoveIfExists<T>(string key);
    void Insert<T>(string key, T data);
  }
}
