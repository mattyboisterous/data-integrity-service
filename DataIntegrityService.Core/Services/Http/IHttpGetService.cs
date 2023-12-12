using DataIntegrityService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Services.Http
{
  public interface IHttpGetService
  {
    string Url { get; set; }

    Task<DataResponse<T>> HttpGet<T>(string url, HttpMessageHandler handler, CancellationTokenSource tokenSource = null!);
    Task<DataResponse<List<T>>> HttpGetAll<T>(string url, HttpMessageHandler handler, CancellationTokenSource tokenSource = null!);
  }
}
