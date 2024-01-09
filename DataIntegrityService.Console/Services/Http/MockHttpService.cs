using DataIntegrityService.Console.Models;
using DataIntegrityService.Core.Logging;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Models.Interfaces;
using DataIntegrityService.Core.Services.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataIntegrityService.Console.Services.Http
{
  public class MockHttpService : IHttpService
  {
    public required IUserProfile User { get; set; }

    public async Task<IDataResponse<T>> Get<T>(string url, HttpMessageHandler messageHandler, CancellationToken token) where T : IDataModel
    {
      if (typeof(T) == typeof(ProvisionModel))
      {
        var data = new ProvisionModel() { ProvisionId = 1 };

        Logger.Info("MockHttpService", $"Returning 1 item from server.");

        var result = new DataResponse<T>((T)(object)data);
        result.HttpResponseCode = GenerateHttpResponseCode();

        return await Task.FromResult(result);
      }
      if (typeof(T) == typeof(QualityAreaModel))
      {
        var data = new QualityAreaModel() { QualityAreaId = 1, Code = "QA1", Description = "Quality Area 1" };

        Logger.Info("MockHttpService", $"Returning 1 item from server.");

        var result = new DataResponse<T>((T)(object)data);
        result.HttpResponseCode = GenerateHttpResponseCode();

        return await Task.FromResult(result);
      }
      if (typeof(T) == typeof(VisitModel))
      {
        var data = new VisitModel() { VisitId = Guid.NewGuid().ToString() };

        Logger.Info("MockHttpService", $"Returning 1 item from server.");

        var result = new DataResponse<T>((T)(object)data);
        result.HttpResponseCode = GenerateHttpResponseCode();

        return await Task.FromResult(result);
      }

      return default;
    }

    public async Task<IDataResponse<int>> Get(string url, HttpMessageHandler messageHandler, CancellationToken token)
    {
      return await Task.FromResult(new DataResponse<int>(1));
    }

    //public async Task<IDataResponse<T>> Get<T>(string url, HttpMessageHandler messageHandler, CancellationTokenSource tokenSource = null)
    //{
    //  if (typeof(T) == typeof(QualityAreaModel))
    //  {
    //    var testData = (T)(object)new QualityAreaModel() { QualityAreaId = 1, Code = "QA1", Description = "Quality Area 1" };

    //    Logger.Info("MockHttpService", $"Http 200, returning 1 item.");

    //    return await Task.FromResult(new DataResponse<T>(testData));
    //  }

    //  return await Task.FromResult(new DataResponse<T>(default));
    //}

    public async Task<IDataResponse<IEnumerable<T>>> GetAll<T>(string url, HttpMessageHandler messageHandler, CancellationToken token) where T : IDataModel
    {
      if (typeof(T) == typeof(QualityAreaModel))
      {
        var data = new List<QualityAreaModel>() { new QualityAreaModel() { QualityAreaId = 1, Code = "QA1", Description = "Quality Area 1" }, new QualityAreaModel() { QualityAreaId = 2, Code = "QA2", Description = "Quality Area 2" } };

        Logger.Info("MockHttpService", $"Http 200, returning 2 items.");

        var testData = (IEnumerable<T>)(object)data;

        return await Task.FromResult(new DataResponse<IEnumerable<T>>(testData));
      }

      return await Task.FromResult(new DataResponse<IEnumerable<T>>());
    }

    public async Task<IDataResponse<T>> Post<T>(string url, T item, HttpMessageHandler messageHandler) where T : IDataModel
    {
      return await Task.FromResult(new DataResponse<T>(item));
    }

    public async Task<IDataResponse<T>> Put<T>(string url, T item, HttpMessageHandler messageHandler) where T : IDataModel
    {
      return await Task.FromResult(new DataResponse<T>(item));
    }

    public async Task<IDataResponse<int>> Delete(string url, HttpMessageHandler messageHandler)
    {
      return await Task.FromResult(new DataResponse<int>(1));
    }

    /// <summary>
    /// Seeing as we are mocking lets simulate the occasional call failing...
    /// </summary>
    /// <returns></returns>
    private int GenerateHttpResponseCode()
    {
      Random rnd = new Random();
      int num = rnd.Next(0, 100);

      if (num <= 90)
        return 200;
      else if (num <= 95)
        return 400;
      else
        return 500;
    }
  }
}
