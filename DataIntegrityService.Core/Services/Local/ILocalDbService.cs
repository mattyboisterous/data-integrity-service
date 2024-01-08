using DataIntegrityService.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Services.Local
{
    public interface ILocalDbService
  {
    IDataModel GetLocal(string key);
    int Delete<IDataModel>(string key);
    int DeleteAll<IDataModel>();
    int Insert(IDataModel data);
    int Update(IDataModel data);
    int InsertAll(IEnumerable<IDataModel> data);
  }
}
