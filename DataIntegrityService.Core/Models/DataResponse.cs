﻿using DataIntegrityService.Core.Models.Interfaces;

namespace DataIntegrityService.Core.Models
{
  public class DataResponse<T> : IDataResponse<T>
  {
    public DataResponse()
    { }

    public DataResponse(T data)
    {
      Data = data;
    }

    public T Data { get; set; }
    public bool ActionCancelled { get; set; }
    public bool ActionSucceeded { get; set; } = true;
    public int HttpResponseCode { get; set; }
  }
}
