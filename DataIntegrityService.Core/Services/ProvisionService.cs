﻿using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Interfaces;
using DataIntegrityService.Core.Logging;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Services.Http;
using DataIntegrityService.Core.Services.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Providers
{
  public class ProvisionService : IDataService, ILocalCacheService
  {
    private IHttpService HttpService { get; set; }

    public string Key => "Provision";

    public required DataServiceConfiguration Settings { get; set; }

    public bool IsInitialised { get; set; }

    public required string Url { get; set; }

    public ProvisionService(IHttpService httpService)
    {
      HttpService = httpService;
    }

    public void Initialise()
    {
      // do nothing...
      IsInitialised = true;
    }

    #region IDataService members

    public IEnumerable<IDataModel> TransformData(IEnumerable<IDataModel> data)
    {
      // no transformation required...
      Logger.Info("ProvisionService", "No transformation required, returning data.");
      return data;
    }

    public async Task<IDataResponse<IEnumerable<IDataModel>>> GetAllFromServer(HttpMessageHandler messageHandler, CancellationToken cancellationToken)
    {
      var result = await HttpService.GetAll<QualityAreaModel>(Url, messageHandler, cancellationToken);

      return new DataResponse<IEnumerable<IDataModel>>
      {
        Data = result.Data.Cast<IDataModel>(),
        ActionSucceeded = result.ActionSucceeded
      };
    }

    public async Task<IDataResponse<IEnumerable<IDataModel>>> GetAllFromServerByKey(string key, HttpMessageHandler messageHandler, CancellationToken cancellationToken)
    {
      var result = await HttpService.GetAll<QualityAreaModel>(string.Format(Settings.Http.GetAllByKey, key), messageHandler, cancellationToken);

      return new DataResponse<IEnumerable<IDataModel>>
      {
        Data = result.Data.Cast<IDataModel>(),
        ActionSucceeded = result.ActionSucceeded
      };
    }

    #endregion

    #region ILocalCacheService members

    public IEnumerable<IDataModel> GetAllLocal(string key)
    {
      throw new NotImplementedException();
    }

    public void RemoveIfExists(string key)
    {
      // mock response for now...
      Logger.Info("ProvisionService", $"Deleting all from local cache with key '{key}'.");
    }

    public void InsertOrReplace(string key, IEnumerable<IDataModel> data)
    {
      // mock response for now...
      Logger.Info("ProvisionService", $"Inserting {data.Count()} item(s) into local cache with key '{key}'.");
    }

    #endregion
  }
}
