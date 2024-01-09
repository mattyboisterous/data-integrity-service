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
    T GetLocal<T>(string key) where T : IDataModel;
    int Delete<T>(string key) where T : IDataModel;
    int DeleteAll<T>() where T : IDataModel;
    int Insert<T>(T data) where T : IDataModel;
    int Update<T>(T data) where T : IDataModel;
    int InsertAll<T>(IEnumerable<T> data) where T : IDataModel;
  }
}
