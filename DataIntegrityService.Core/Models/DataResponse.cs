using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataIntegrityService.Core.Models
{
  public class DataResponse<T>
  {
    public T Data { get; set; }
    public bool MethodCancelled { get; set; }
    public bool MethodSucceeded { get; set; } = true;
    //public Error Error { get; set; }
    //public bool HasError
    //{
    //  get { return Error != null; }
    //}
  }
}
