using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataIntegrityService.Core.Models
{
  public interface IDataResponse<T>
  {
    T Data { get; set; }
    bool ActionCancelled { get; set; }
    bool ActionSucceeded { get; set; }
    int HttpResponseCode { get; set; }
  }

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
