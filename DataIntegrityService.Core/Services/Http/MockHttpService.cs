using DataIntegrityService.Core.Logging;
using DataIntegrityService.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataIntegrityService.Core.Services.Http
{
  public class MockHttpService : IHttpService
  {
    public async Task<IDataResponse<T>> Get<T>(string url, HttpMessageHandler handler, CancellationTokenSource tokenSource = null)
    {
      if (typeof(T) == typeof(QualityAreaModel))
      {
        var testData = (T)(object)new QualityAreaModel() { Code = "QA1", Description = "Quality Area 1" };

        Logger.Info("MockHttpService", $"Http 200, returning 1 item.");

        return await Task.FromResult(new DataResponse<T>(testData));
      }

      return await Task.FromResult(new DataResponse<T>(default));
    }

    public async Task<IDataResponse<IEnumerable<T>>> GetAll<T>(string url, HttpMessageHandler handler, CancellationTokenSource tokenSource = null)
    {
      if (typeof(T) == typeof(QualityAreaModel))
      {
        var data = new List<QualityAreaModel>() { new QualityAreaModel() { Code = "QA1", Description = "Quality Area 1" }, new QualityAreaModel() { Code = "QA2", Description = "Quality Area 2" } };

        Logger.Info("MockHttpService", $"Http 200, returning 2 items.");

        var testData = (IEnumerable<T>)(object)data;

        return await Task.FromResult(new DataResponse<IEnumerable<T>>(testData));
      }

      return await Task.FromResult(new DataResponse<IEnumerable<T>>());
    }

    public async Task<IDataResponse<T>> Post<T>(string url, T item, HttpMessageHandler handler)
    {
      return await Task.FromResult(new DataResponse<T>(item));
    }

    public async Task<IDataResponse<T>> Put<T>(string url, T item, string id, HttpMessageHandler handler)
    {
      return await Task.FromResult(new DataResponse<T>(item));
    }

    public async Task<IDataResponse<int>> Delete<T>(string url, string id, HttpMessageHandler handler)
    {
      return await Task.FromResult(new DataResponse<int>(1));
    }
  }
}
