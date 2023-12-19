using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Services.Http
{
  /// <summary>
  /// Hook to allow message handler pipelines to be passed to the data service.
  /// </summary>
  public class HttpMessageHandlerService : IHttpMessageHandlerService
  {
    public HttpMessageHandler GetMessageHandler()
    {
      return null;
    }
  }
}
