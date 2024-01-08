﻿using DataIntegrityService.Console.Models;
using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Logging;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Models.Interfaces;
using DataIntegrityService.Core.Services.Http;
using DataIntegrityService.Core.Services.Interfaces;
using DataIntegrityService.Core.Services.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Console.Providers
{
    public class VisitService : IDataService, ILocalDbService
  {
    private IHttpService HttpService { get; set; }
    private IHttpMessageHandlerService HttpMessageHandlerService { get; set; }

    public string Key => "Visit";
    public required DataServiceConfiguration Settings { get; set; }
    public bool IsInitialised { get; set; }

    public VisitService(
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

    #region IDataService Members

    public IDataModel TransformData(IDataModel data)
    {
      // no transformations...
      return data;
    }

    public IEnumerable<IDataModel> TransformData(IEnumerable<IDataModel> data)
    {
      // no transformation required...
      Logger.Info("VisitService", "No transformation required, returning data.");
      return data;
    }

    public async Task<IDataResponse<IDataModel>> GetFromServer(string id, CancellationToken cancellationToken)
    {
      var result = await HttpService.Get<VisitModel>(string.Format(Settings.Http.Get, id), HttpMessageHandlerService.GetMessageHandler(), cancellationToken);

      return new DataResponse<IDataModel>
      {
        Data = (IDataModel)result.Data,
        ActionSucceeded = result.ActionSucceeded
      };
    }

    public Task<IDataResponse<IEnumerable<IDataModel>>> GetAllFromServer(CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task<IDataResponse<IEnumerable<IDataModel>>> GetAllFromServerByKey(string key, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public async Task<IDataResponse<IDataModel>> CreateOnServer(IDataModel model, CancellationToken cancellationToken)
    {
      var result = await HttpService.Post(Settings.Http.Post, (VisitModel)model, HttpMessageHandlerService.GetMessageHandler());

      return new DataResponse<IDataModel>
      {
        Data = (IDataModel)result.Data,
        ActionSucceeded = result.ActionSucceeded
      };
    }

    public async Task<IDataResponse<IDataModel>> UpdateOnServer(IDataModel model, CancellationToken cancellationToken)
    {
      var result = await HttpService.Put(Settings.Http.Put, (VisitModel)model, HttpMessageHandlerService.GetMessageHandler());

      return new DataResponse<IDataModel>
      {
        Data = (IDataModel)result.Data,
        ActionSucceeded = result.ActionSucceeded
      };
    }

    public Task<IDataResponse<bool>> DeleteFromServer(string key, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    #endregion

    #region ILocalDbService Members

    public IDataModel GetLocal(string key)
    {
      // mock response for now...
      Logger.Info("VisitService", $"Fetching record with id '{key}' from local Db...");
      return new VisitModel() { VisitId = 1 };
    }

    public int DeleteAll<IDataModel>()
    {
      throw new NotImplementedException();
    }

    public int InsertAll(IEnumerable<IDataModel> data)
    {
      throw new NotImplementedException();
    }

    public int Delete<IDataModel>(string key)
    {
      throw new NotImplementedException();
    }

    public int Insert(IDataModel data)
    {
      throw new NotImplementedException();
    }

    public int Update(IDataModel data)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
