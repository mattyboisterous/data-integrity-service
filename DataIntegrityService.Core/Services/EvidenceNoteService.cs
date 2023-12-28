using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Logging;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Models.Interfaces;
using DataIntegrityService.Core.Services.Http;
using DataIntegrityService.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Providers
{
    public class EvidenceNoteService : IDataService
  {
    private IHttpService HttpService { get; set; }
    private IHttpMessageHandlerService HttpMessageHandlerService { get; set; }

    public string Key => "EvidenceNote";
    public required DataServiceConfiguration Settings { get; set; }
    public bool IsInitialised { get; set; }

    public EvidenceNoteService(
      IHttpService httpService,
      IHttpMessageHandlerService httpMessageHandlerService)
    {
      HttpService = httpService;
      HttpMessageHandlerService = httpMessageHandlerService;
    }

    public async Task Initialise()
    {
      IsInitialised = true;
      await Task.CompletedTask;
    }

    public IDataModel TransformData(IDataModel data)
    {
      // no transformations...
      return data;
    }

    public IEnumerable<IDataModel> TransformData(IEnumerable<IDataModel> data)
    {
      // no transformation required...
      Logger.Info("EvidenceNoteService", "No transformation required, returning data.");
      return data;
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

    public Task<IDataResponse<IDataModel>> CreateOnServer(IDataModel model, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task<IDataResponse<IDataModel>> UpdateOnServer(IDataModel model, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task<IDataResponse<bool>> DeleteFromServer(string key, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }
  }
}
