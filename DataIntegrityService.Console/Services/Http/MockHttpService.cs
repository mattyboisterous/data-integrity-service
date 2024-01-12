using DataIntegrityService.Console.Models;
using DataIntegrityService.Core.Logging;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Models.Interfaces;
using DataIntegrityService.Core.Services.Http;

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

        var result = new DataResponse<T>((T)(object)data);
        result.HttpResponseCode = GenerateHttpResponseCode();

        Logger.Info("MockHttpService", $"GET => Returning model from server. HttpResponseCode => {result.HttpResponseCode}");

        return await Task.FromResult(result);
      }
      if (typeof(T) == typeof(QualityAreaModel))
      {
        var data = new QualityAreaModel() { QualityAreaId = 1, Code = "QA1", Description = "Quality Area 1" };

        var result = new DataResponse<T>((T)(object)data);
        result.HttpResponseCode = GenerateHttpResponseCode();

        Logger.Info("MockHttpService", $"GET => Returning model from server. HttpResponseCode => {result.HttpResponseCode}");

        return await Task.FromResult(result);
      }
      if (typeof(T) == typeof(VisitModel))
      {
        var data = new VisitModel() { VisitId = Guid.NewGuid().ToString() };

        var result = new DataResponse<T>((T)(object)data);
        result.HttpResponseCode = GenerateHttpResponseCode();

        Logger.Info("MockHttpService", $"GET => Returning model from server. HttpResponseCode => {result.HttpResponseCode}");

        return await Task.FromResult(result);
      }
      if (typeof(T) == typeof(MemoModel))
      {
        var data = new MemoModel() { MemoId = 1 };

        var result = new DataResponse<T>((T)(object)data);
        result.HttpResponseCode = GenerateHttpResponseCode();

        Logger.Info("MockHttpService", $"GET => Returning model from server. HttpResponseCode => {result.HttpResponseCode}");

        return await Task.FromResult(result);
      }

      return default;
    }

    public async Task<IDataResponse<int>> Get(string url, HttpMessageHandler messageHandler, CancellationToken token)
    {
      var result = new DataResponse<int>(1);
      result.HttpResponseCode = GenerateHttpResponseCode();

      Logger.Info("MockHttpService", $"GET => Returning model from server. HttpResponseCode => {result.HttpResponseCode}");

      return await Task.FromResult(result);
    }

    public async Task<IDataResponse<IEnumerable<T>>> GetAll<T>(string url, HttpMessageHandler messageHandler, CancellationToken token) where T : IDataModel
    {
      if (typeof(T) == typeof(StaticDataChangeTrackingModel))
      {
        var data = new List<StaticDataChangeTrackingModel>()
        {
          new StaticDataChangeTrackingModel() { Id = "1", DatasetName = "QualityAreas", Version = "a6417e4f-49c8-483f-aa61-9f722d700204" },
          new StaticDataChangeTrackingModel() { Id = "2", DatasetName = "Provisions", Version = "fb5e9e8c-57b5-4180-93d8-7c5fff95abdc" }
        };

        var result = new DataResponse<IEnumerable<T>>((IEnumerable<T>)(object)data);
        result.HttpResponseCode = GenerateHttpResponseCode();

        Logger.Info("MockHttpService", $"GET => Returning models from server. HttpResponseCode => {result.HttpResponseCode}");

        return await Task.FromResult(result);
      }
      if (typeof(T) == typeof(QualityAreaModel))
      {
        var data = new List<QualityAreaModel>() { new QualityAreaModel() { QualityAreaId = 1, Code = "QA1", Description = "Quality Area 1" }, new QualityAreaModel() { QualityAreaId = 2, Code = "QA2", Description = "Quality Area 2" } };

        var result = new DataResponse<IEnumerable<T>>((IEnumerable<T>)(object)data);
        result.HttpResponseCode = GenerateHttpResponseCode();

        Logger.Info("MockHttpService", $"GET => Returning models from server. HttpResponseCode => {result.HttpResponseCode}");

        return await Task.FromResult(result);
      }

      return await Task.FromResult(new DataResponse<IEnumerable<T>>());
    }

    public async Task<IDataResponse<T>> Post<T>(string url, T item, HttpMessageHandler messageHandler) where T : IDataModel
    {
      var result = new DataResponse<T>((T)(object)item);
      result.HttpResponseCode = GenerateHttpResponseCode();

      Logger.Info("MockHttpService", $"POST => Returning model from server. HttpResponseCode => {result.HttpResponseCode}");

      return await Task.FromResult(result);
    }

    public async Task<IDataResponse<T>> Put<T>(string url, T item, HttpMessageHandler messageHandler) where T : IDataModel
    {
      var result = new DataResponse<T>((T)(object)item);
      result.HttpResponseCode = GenerateHttpResponseCode();

      Logger.Info("MockHttpService", $"PUT => Returning model from server. HttpResponseCode => {result.HttpResponseCode}");

      return await Task.FromResult(result);
    }

    public async Task<IDataResponse<int>> Delete(string url, HttpMessageHandler messageHandler)
    {
      var result = new DataResponse<int>(1);
      result.HttpResponseCode = GenerateHttpResponseCode();

      Logger.Info("MockHttpService", $"DELETE => Returning count of deleted items from server. HttpResponseCode => {result.HttpResponseCode}");

      return await Task.FromResult(result);
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
