using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataIntegrityService.Core.Models.Interfaces;

namespace DataIntegrityService.Core.Models
{
    public class DataChangeTrackingModel : StaticDataChangeTrackingModel
  {
    public new string Key => Id;

    public required string UserId { get; set; }
    public required string ItemKey { get; set; }
    public required string Action { get; set; }
    public required int Attempts { get; set; }
    public DateTime LastAttempt { get; set; }
    public required DateTime Created { get; set; }
  }
}
