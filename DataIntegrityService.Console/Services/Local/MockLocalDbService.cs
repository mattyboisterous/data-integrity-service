using DataIntegrityService.Console.Models;
using DataIntegrityService.Core.Logging;
using DataIntegrityService.Core.Models.Interfaces;
using DataIntegrityService.Core.Services.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataIntegrityService.Console.Services.Local
{
  public class MockLocalDbService : ILocalDbService
  {
    public T GetLocal<T>(string key) where T : IDataModel
    {
      if (typeof(T) == typeof(ProvisionModel))
      {
        var data = new ProvisionModel() { ProvisionId = int.Parse(key) };

        Logger.Info("MockLocalDbService", $"Returning 1 item from local Db.");

        return (T)(object)data;
      }
      if (typeof(T) == typeof(QualityAreaModel))
      {
        var data = new QualityAreaModel() { QualityAreaId = int.Parse(key), Code = "QA1", Description = "Quality Area 1" };

        Logger.Info("MockLocalDbService", $"Returning 1 item from local Db.");

        return (T)(object)data;
      }
      if (typeof(T) == typeof(VisitModel))
      {
        var data = new VisitModel() { VisitId = int.Parse(key) };

        Logger.Info("MockLocalDbService", $"Returning 1 item from local Db.");

        return (T)(object)data;
      }

      return default;
    }

    public int Insert<T>(T data) where T : IDataModel
    {
      Logger.Info("MockLocalDbService", $"Inserting item with key '{data.Key}' in local Db.");

      return 1;
    }

    public int InsertAll<T>(IEnumerable<T> data) where T : IDataModel
    {
      Logger.Info("MockLocalDbService", $"Inserting items in local Db.");

      return data.Count();
    }

    public int Update<T>(T data) where T : IDataModel
    {
      Logger.Info("MockLocalDbService", $"Updating item with key '{data.Key}' in local Db.");

      return 1;
    }

    public int Delete<T>(string key) where T : IDataModel
    {
      Logger.Info("MockLocalDbService", $"Deleting item with key '{key}' in local Db.");

      return 1;
    }

    public int DeleteAll<T>() where T : IDataModel
    {
      Logger.Info("MockLocalDbService", $"Deleting all items in local Db.");

      return 7;
    }
  }
}
