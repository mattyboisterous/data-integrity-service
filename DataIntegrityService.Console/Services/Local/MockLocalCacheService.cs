using DataIntegrityService.Console.Models;
using DataIntegrityService.Core.Logging;
using DataIntegrityService.Core.Models.Interfaces;
using DataIntegrityService.Core.Services.Local;

namespace DataIntegrityService.Console.Services.Local
{
  public class MockLocalCacheService : ILocalCacheService
  {
    public T GetLocal<T>(string key) where T : IDataModel
    {
      if (typeof(T) == typeof(ProvisionModel))
      {
        var data = new ProvisionModel() { ProvisionId = int.Parse(key) };

        Logger.Info("MockLocalCacheService", $"Returning 1 item from local cache.");

        return (T)(object)data;
      }
      if (typeof(T) == typeof(QualityAreaModel))
      {
        var data = new QualityAreaModel() { QualityAreaId = int.Parse(key), Code = "QA1", Description = "Quality Area 1" };

        Logger.Info("MockLocalCacheService", $"Returning 1 item from local cache.");

        return (T)(object)data;
      }
      if (typeof(T) == typeof(VisitModel))
      {
        var data = new VisitModel() { VisitId = int.Parse(key) };

        Logger.Info("MockLocalCacheService", $"Returning 1 item from local cache.");

        return (T)(object)data;
      }

      return default;
    }

    public IEnumerable<T> GetAllLocal<T>(string key) where T : IDataModel
    {
      if (typeof(T) == typeof(ProvisionModel))
      {
        var data = new List<ProvisionModel>() { new ProvisionModel() { ProvisionId = 1 }, new ProvisionModel() { ProvisionId = 2 }, new ProvisionModel() { ProvisionId = 3 } };

        Logger.Info("MockLocalCacheService", $"Returning 3 items from local cache.");

        return (IEnumerable<T>)(object)data;
      }
      if (typeof(T) == typeof(QualityAreaModel))
      {
        var data = new List<QualityAreaModel>() { new QualityAreaModel() { QualityAreaId = 1, Code = "QA1", Description = "Quality Area 1" }, new QualityAreaModel() { QualityAreaId = 2, Code = "QA2", Description = "Quality Area 2" } };

        Logger.Info("MockLocalCacheService", $"Returning 2 items from local cache.");

        return (IEnumerable<T>)(object)data;
      }
      if (typeof(T) == typeof(VisitModel))
      {
        var data = new List<VisitModel>() { new VisitModel() { VisitId = 1 }, new VisitModel() { VisitId = 2 }, new VisitModel() { VisitId = 3 } };

        Logger.Info("MockLocalCacheService", $"Returning 3 items from local cache.");

        return (IEnumerable<T>)(object)data;
      }

      return Enumerable.Empty<T>();
    }

    public void InsertOrReplace<T>(string key, T data) where T : IDataModel
    {
      Logger.Info("MockLocalCacheService", $"Upserting item with key {key} in local cache.");
    }

    public void InsertOrReplace<T>(string key, IEnumerable<T> data) where T : IDataModel
    {
      Logger.Info("MockLocalCacheService", $"Upserting items with key {key} in local cache.");
    }

    public void RemoveIfExists(string key)
    {
      Logger.Info("MockLocalCacheService", $"Removed item with key {key} from local cache.");
    }
  }
}
