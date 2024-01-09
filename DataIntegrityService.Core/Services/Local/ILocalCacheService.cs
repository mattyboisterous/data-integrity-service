using DataIntegrityService.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Services.Local
{
    public interface ILocalCacheService
  {
    T GetLocal<T>(string key) where T : IDataModel;
    IEnumerable<T> GetAllLocal<T>(string key) where T : IDataModel;
    void InsertOrReplace<T>(string key, T data) where T : IDataModel;
    void InsertOrReplace<T>(string key, IEnumerable<T> data) where T : IDataModel;
    void RemoveIfExists(string key);
  }
}
