using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Interfaces;
using DataIntegrityService.Core.Logging;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Services.Http;
using DataIntegrityService.Core.Services.Local;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

      Logger.Info("Data service initialised.");
    }

    public IEnumerable<IDataModel> TransformData(IEnumerable<IDataModel> data)
    {
      // no transformation required...
      Logger.Info("No transformation required, returning data.");
      return data;
    }

    #region IHttpGetService members

    public Task<DataResponse<IDataModel>> HttpGet(string url, HttpMessageHandler handler, CancellationTokenSource tokenSource = null)
    {
      throw new NotImplementedException();
    }

    public async Task<DataResponse<IEnumerable<IDataModel>>> HttpGetAll(string url, HttpMessageHandler handler, CancellationTokenSource tokenSource = null)
    {
      // mock response for now...
      Logger.Info("Fetching data from Api...");
      var data = new List<QualityAreaModel>() { new QualityAreaModel() { Code = "QA1", Description = "Quality Area 1" }, new Models.QualityAreaModel() { Code = "QA2", Description = "Quality Area 2" } };

      Logger.Info($"{data.Count} item(s) found, returning...");
      return await Task.FromResult(new DataResponse<IEnumerable<IDataModel>>(data));
    }

    #endregion

    #region ILocalDbService

    public int InsertAll(IEnumerable<IDataModel> data)
    {
      // mock response for now...
      Logger.Info($"Inserting {data.Count()} item(s) into local DB.");
      return data.Count();
    }

    public int DeleteAll<T>()
    {
      // mock response for now...
      Logger.Info($"Deleting all from local DB.");
      return 3;
    }

    #endregion
  }
}
