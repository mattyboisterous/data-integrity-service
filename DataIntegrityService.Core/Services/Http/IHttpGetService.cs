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

    Task<DataResponse<IDataModel>> HttpGet(string url, HttpMessageHandler handler, CancellationTokenSource tokenSource = null!);
    Task<DataResponse<IEnumerable<IDataModel>>> HttpGetAll(string url, HttpMessageHandler handler, CancellationTokenSource tokenSource = null!);
  }
}
