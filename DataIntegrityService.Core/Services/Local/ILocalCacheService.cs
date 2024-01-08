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
    IDataModel GetLocal(string key);
    IEnumerable<IDataModel> GetAllLocal(string key);
    void RemoveIfExists(string key);
    void InsertOrReplace(string key, IDataModel data);
    void InsertOrReplace(string key, IEnumerable<IDataModel> data);
  }
}
