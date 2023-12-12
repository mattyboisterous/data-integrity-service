using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Interfaces;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Services.Http;
using DataIntegrityService.Core.Services.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Services
{
  public class QualityAreaService : IDataService, IHttpGetService, ILocalDbService
  {
    public string Key => "QualityArea";

    public required DataServiceConfiguration Settings { get; set; }

    public bool IsInitialised { get; set; }

    public required string Url { get; set; }

    public void Initialise()
    {
      Url = Settings.Http.Get!;
      IsInitialised = true;
    }

    #region IHttpGetService members

    public Task<DataResponse<T>> HttpGet<T>(string url, HttpMessageHandler handler, CancellationTokenSource tokenSource = null)
    {
      throw new NotImplementedException();
    }

    public async Task<DataResponse<List<T>>> HttpGetAll<T>(string url, HttpMessageHandler handler, CancellationTokenSource tokenSource = null) where T : IDataModel
    {
      // todo: mock response for now...
      var data = new List<QualityAreaModel>() { new QualityAreaModel() { Code = "QA1", Description = "Quality Area 1" }, new Models.QualityAreaModel() { Code = "QA2", Description = "Quality Area 2" } };

      return await Task.FromResult(new DataResponse<List<QualityAreaModel>>() { Data = data, MethodSucceeded = true });

    }

    public T GetModel<T>(string key) where T : IDataModel
    {
      if (key == "x")
        return ((T)(IDataModel)new QualityAreaModel() { Code = "x", Description = "x" });

      throw new Exception();
    }

    #endregion

    #region ILocalDbService

    public int InsertAll<T>(List<T> data)
    {
      throw new NotImplementedException();
    }

    public int DeleteAll<T>()
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
