using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Services.Http
{
  public interface IHttpGetService
  {
    Task<T> HttpGet<T>(string url, HttpMessageHandler handler, CancellationTokenSource tokenSource = null!);
  }
}
