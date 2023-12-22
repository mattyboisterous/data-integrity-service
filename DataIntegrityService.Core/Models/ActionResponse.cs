using DataIntegrityService.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Models
{
  public class ActionResponse : IActionResponse
  {
    public bool ActionCancelled { get; set; }
    public bool ActionSucceeded { get; set; } = true;
    public int HttpResponseCode { get; set; }
  }
}
