using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Services.Http
{
  public interface IHttpMessageHandlerService
  {
    HttpMessageHandler GetMessageHandler();
  }
}
