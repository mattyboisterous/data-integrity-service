using DataIntegrityService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataIntegrityService.Core.Services.Http
{
  public class HttpService : IHttpService
  {
    public IUserProfile User { get; set; }

    public Task<IDataResponse<T>> Get<T>(string url, HttpMessageHandler handler, CancellationToken token)
    {
      throw new NotImplementedException();
    }

    public Task<IDataResponse<IEnumerable<T>>> GetAll<T>(string url, HttpMessageHandler handler, CancellationToken token)
    {
      throw new NotImplementedException();
    }

    public Task<IDataResponse<T>> Post<T>(string url, T item, HttpMessageHandler handler)
    {
      throw new NotImplementedException();
    }

    public Task<IDataResponse<T>> Put<T>(string url, T item, HttpMessageHandler handler)
    {
      throw new NotImplementedException();
    }

    public Task<IDataResponse<int>> Delete<T>(string url, HttpMessageHandler handler)
    {
      throw new NotImplementedException();
    }

    public Task<IDataResponse<int>> Get(string url, HttpMessageHandler messageHandler, CancellationToken token)
    {
      throw new NotImplementedException();
    }

    //private HttpClient GetClient(HttpMessageHandler handler)
    //{
    //  var currentPlatform = DeviceInfo.Platform == DevicePlatform.iOS ? "iOS" : "Android";
    //  var currentVersion = $"R4Q {VersionTracking.CurrentVersion}";

    //  var client = handler == null ? new HttpClient() : new HttpClient(handler);
    //  client.BaseAddress = new Uri(AppSettings.Api.HostName);

    //  if (_userProfile != null)
    //  {
    //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userProfile.AccessToken);
    //  }
    //  client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
    //  client.DefaultRequestHeaders.Add("X-Api-Key", AppSettings.Api.ApiKey);

    //  if (!string.IsNullOrEmpty(currentPlatform))
    //    client.DefaultRequestHeaders.Add("Sec-CH-UA-Platform", currentPlatform);
    //  if (!string.IsNullOrEmpty(currentVersion))
    //    client.DefaultRequestHeaders.Add("Sec-CH-UA-Full-Version", currentVersion);

    //  return client;
    //}

    //private async Task<DataResponse<T>> ProcessResponse<T>(HttpResponseMessage response)
    //{
    //  if (response.IsSuccessStatusCode)
    //  {
    //    //var data = await response.Content.ReadAsAsync<T>();
    //    var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

    //    if ((typeof(T) == typeof(string)))
    //      return new DataResponse<T> { Data = (T)(object)responseText };
    //    else
    //    {
    //      var data = JsonSerializer.Deserialize<T>(responseText);

    //      return new DataResponse<T> { Data = data };
    //    }
    //  }
    //  else
    //    return new DataResponse<T>() { Error = new Error() { ApiStatus = response.StatusCode.ToString() } };
    //}
  }
}
