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
    public class VisitService : IDataService
  {
    private IHttpService HttpService { get; set; }
    private IHttpMessageHandlerService HttpMessageHandlerService { get; set; }
    private ILocalDbService DbService { get; set; }

    public string Key => "Visit";
    public required DataServiceConfiguration Settings { get; set; }
    public bool IsInitialised { get; set; }

    public VisitService(
      IHttpService httpService,
      IHttpMessageHandlerService httpMessageHandlerService,
      ILocalDbService dbService)
    {
      HttpService = httpService;
      HttpMessageHandlerService = httpMessageHandlerService;
      DbService = dbService;
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
      Logger.Info("VisitService", "No transformation required, returning data as is.");
      return data;
    }

    public IEnumerable<IDataModel> TransformData(IEnumerable<IDataModel> data)
    {
      // no transformation required...
      Logger.Info("VisitService", "No transformation required, returning data as is.");
      return data;
    }

    public async Task<IDataResponse<IDataModel>> GetFromServer(string id, CancellationToken cancellationToken)
    {
      var result = await HttpService.Get<VisitModel>(string.Format(Settings.Http.Get, id), HttpMessageHandlerService.GetMessageHandler(), cancellationToken);

      return new DataResponse<IDataModel>
      {
        Data = (IDataModel)result.Data,
        HttpResponseCode = result.HttpResponseCode
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
        HttpResponseCode = result.HttpResponseCode
      };
    }

    public async Task<IDataResponse<IDataModel>> UpdateOnServer(IDataModel model, CancellationToken cancellationToken)
    {
      var result = await HttpService.Put(Settings.Http.Put, (VisitModel)model, HttpMessageHandlerService.GetMessageHandler());

      return new DataResponse<IDataModel>
      {
        Data = (IDataModel)result.Data,
        HttpResponseCode = result.HttpResponseCode
      };
    }

    public Task<IDataResponse<bool>> DeleteFromServer(string key, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public T GetLocal<T>(string key) where T : IDataModel
    {
      return DbService.GetLocal<T>(key);
    }

    public int InsertLocal<T>(T data) where T : IDataModel
    {
      return DbService.Insert<T>(data);
    }

    public int UpdateLocal<T>(T data) where T : IDataModel
    {
      return DbService.Update<T>(data);
    }

    public int InsertAllLocal<T>(IEnumerable<T> data) where T : IDataModel
    {
      throw new NotImplementedException();
    }

    public int DeleteLocal<T>(string key) where T : IDataModel
    {
      throw new NotImplementedException();
    }

    public int DeleteAllLocal<T>() where T : IDataModel
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
