using DataIntegrityService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Services.Http
{
  public interface IHttpService
  {
    Task<IDataResponse<T>> Get<T>(string url, HttpMessageHandler handler, CancellationTokenSource tokenSource = null!);
    Task<IDataResponse<IEnumerable<T>>> GetAll<T>(string url, HttpMessageHandler handler, CancellationTokenSource tokenSource = null!);
    Task<IDataResponse<T>> Put<T>(string url, T item, string id, HttpMessageHandler handler);
    Task<IDataResponse<T>> Post<T>(string url, T item, HttpMessageHandler handler);
    Task<IDataResponse<int>> Delete<T>(string url, string id, HttpMessageHandler handler);
  }
}
