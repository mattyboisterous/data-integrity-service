using DataIntegrityService.Core.Models.Interfaces;
using DataIntegrityService.Core.Services.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Console.Services.Local
{
  public class MockLocalCacheService : ILocalCacheService
  {
    public IEnumerable<IDataModel> GetAllLocal(string key)
    {
      throw new NotImplementedException();
    }

    public IDataModel GetLocal(string key)
    {
      throw new NotImplementedException();
    }

    public void InsertOrReplace(string key, IEnumerable<IDataModel> data)
    {
      throw new NotImplementedException();
    }

    public void InsertOrReplace(string key, IDataModel data)
    {
      throw new NotImplementedException();
    }

    public void RemoveIfExists(string key)
    {
      throw new NotImplementedException();
    }
  }
}
