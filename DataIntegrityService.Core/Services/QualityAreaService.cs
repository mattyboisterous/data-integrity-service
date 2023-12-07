using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Interfaces;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Services.Http;
using DataIntegrityService.Core.Services.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Services
{
  public class QualityAreaService : IDataService, IHttpGetService, ILocalDbService
  {
    public string Key => "QualityArea";

    public required DataServiceConfiguration Settings { get; set; }
    
    public required string Url { get; set; }

    public void Initialise()
    {
      Url = Settings.Http.Get!;
    }

    #region IHttpGetService members

    public Task<DataResponse<List<T>>> HttpGetAll<T>(string url, HttpMessageHandler handler, CancellationTokenSource tokenSource = null)
    {
      throw new NotImplementedException();
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
