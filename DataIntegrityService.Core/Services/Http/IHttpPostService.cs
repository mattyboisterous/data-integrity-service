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
    Task<DataResponse<IDataModel>> HttpPost(string url, IDataModel item, HttpMessageHandler handler);
  }
}
