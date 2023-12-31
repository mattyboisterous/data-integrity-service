﻿using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Services.Interfaces
{
  public interface IDataService : IService
  {
    DataServiceConfiguration Settings { get; set; }

    IDataModel TransformData(IDataModel data);
    IEnumerable<IDataModel> TransformData(IEnumerable<IDataModel> data);

    Task<IDataResponse<IDataModel>> GetFromServer(string id, CancellationToken cancellationToken);
    Task<IDataResponse<IEnumerable<IDataModel>>> GetAllFromServer(CancellationToken cancellationToken);
    Task<IDataResponse<IEnumerable<IDataModel>>> GetAllFromServerByKey(string key, CancellationToken cancellationToken);

    Task<IDataResponse<IDataModel>> CreateOnServer(IDataModel model, CancellationToken cancellationToken);
    Task<IDataResponse<IDataModel>> UpdateOnServer(IDataModel model, CancellationToken cancellationToken);
    Task<IDataResponse<bool>> DeleteFromServer(string key, CancellationToken cancellationToken);

    T GetLocal<T>(string key) where T : IDataModel;
    int InsertLocal<T>(T data) where T : IDataModel;
    int UpdateLocal<T>(T data) where T : IDataModel;
    int InsertAllLocal<T>(IEnumerable<T> data) where T : IDataModel;
    int DeleteLocal<T>(string key) where T : IDataModel;
    int DeleteAllLocal<T>() where T : IDataModel;
  }
}
