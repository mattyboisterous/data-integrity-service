﻿using DataIntegrityService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Services.Http
{
  public interface IHttpService
  {
    IUserProfile User { get; set; }
    Task<IDataResponse<int>> Get(string url, HttpMessageHandler messageHandler, CancellationToken token);
    Task<IDataResponse<T>> Get<T>(string url, HttpMessageHandler messageHandler, CancellationToken token);
    Task<IDataResponse<IEnumerable<T>>> GetAll<T>(string url, HttpMessageHandler messageHandler, CancellationToken token);
    Task<IDataResponse<T>> Put<T>(string url, T item, HttpMessageHandler messageHandler);
    Task<IDataResponse<T>> Post<T>(string url, T item, HttpMessageHandler messageHandler);
    Task<IDataResponse<int>> Delete<T>(string url, HttpMessageHandler messageHandler);
  }
}
