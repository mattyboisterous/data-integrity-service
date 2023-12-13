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
    Task<DataResponse<IDataModel>> HttpPut(string url, IDataModel item, string id, HttpMessageHandler handler);
  }
}
