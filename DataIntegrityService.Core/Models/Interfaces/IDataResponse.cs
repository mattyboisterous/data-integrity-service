using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Models.Interfaces
{
  public interface IDataResponse<T> : IActionResponse
  {
    T Data { get; set; }
  }
}
