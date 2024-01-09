using DataIntegrityService.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Console.Models
{
  public class VisitModel : IDataModel
  {
    public string Key => VisitId;

    public required string VisitId { get; set; }
  }
}
