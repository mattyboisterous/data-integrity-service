using DataIntegrityService.Core.Models.Interfaces;
using DataIntegrityService.Core.Services.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Console.Services.Local
{
  public class MockLocalDbService : ILocalDbService
  {
    public int Delete<IDataModel>(string key)
    {
      throw new NotImplementedException();
    }

    public int DeleteAll<IDataModel>()
    {
      return 3;
    }

    public IDataModel GetLocal(string key)
    {
      throw new NotImplementedException();
    }

    public int Insert(IDataModel data)
    {
      throw new NotImplementedException();
    }

    public int InsertAll(IEnumerable<IDataModel> data)
    {
      throw new NotImplementedException();
    }

    public int Update(IDataModel data)
    {
      throw new NotImplementedException();
    }
  }
}
