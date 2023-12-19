﻿using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Interfaces;
using DataIntegrityService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Providers
{
  public class VisitService : IDataService
  {
    public string Key => "Visit";
    public required DataServiceConfiguration Settings { get; set; }
    public bool IsInitialised { get; set; }

    public void Initialise()
    {
      throw new NotImplementedException();
    }

    public IEnumerable<IDataModel> TransformData(IEnumerable<IDataModel> data)
    {
      throw new NotImplementedException();
    }

    public Task<IDataResponse<IEnumerable<IDataModel>>> GetAllFromServer(HttpMessageHandler messageHandler, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task<IDataResponse<IEnumerable<IDataModel>>> GetAllFromServerByKey(string key, HttpMessageHandler messageHandler, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }
  }
}
