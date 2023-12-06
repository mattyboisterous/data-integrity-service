using DataIntegrityService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Services.Http
{
  public interface IHttpPutService
  {
    Task<DataResponse<T>> HttpPut<T>(string url, T item, string id, HttpMessageHandler handler);
  }
}
