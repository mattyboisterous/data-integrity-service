using DataIntegrityService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Services.Http
{
  public interface IHttpPostService
  {
    Task<DataResponse<T1>> HttpPost<T, T1>(string url, T item, HttpMessageHandler handler);
  }
}
