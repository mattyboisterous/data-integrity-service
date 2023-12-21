using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Interfaces;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Services.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Providers
{
  public class MemoService : IDataService
  {
    private IHttpService HttpService { get; set; }
    private IHttpMessageHandlerService HttpMessageHandlerService { get; set; }

    public string Key => "Memo";
    public required DataServiceConfiguration Settings { get; set; }
    public bool IsInitialised { get; set; }

    public MemoService(
      IHttpService httpService,
      IHttpMessageHandlerService httpMessageHandlerService)
    {
      HttpService = httpService;
      HttpMessageHandlerService = httpMessageHandlerService;
    }

    public void Initialise()
    {
      throw new NotImplementedException();
    }

    public IDataModel TransformData(IDataModel data)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<IDataModel> TransformData(IEnumerable<IDataModel> data)
    {
      throw new NotImplementedException();
    }

    public Task<IDataResponse<IDataModel>> GetFromServer(string id, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task<IDataResponse<IEnumerable<IDataModel>>> GetAllFromServer(CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }
    public Task<IDataResponse<IEnumerable<IDataModel>>> GetAllFromServerByKey(string key, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task<IDataResponse<IDataModel>> PushToServer(IDataModel model, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }
  }
}
