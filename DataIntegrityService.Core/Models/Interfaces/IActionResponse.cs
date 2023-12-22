using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Models.Interfaces
{
  public interface IActionResponse
  {
    bool ActionCancelled { get; set; }
    bool ActionSucceeded { get; set; }
    int HttpResponseCode { get; set; }
  }
}
