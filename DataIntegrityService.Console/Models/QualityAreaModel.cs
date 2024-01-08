using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataIntegrityService.Core.Models.Interfaces;

namespace DataIntegrityService.Console.Models
{
    public class QualityAreaModel : IDataModel
  {
    public string Key => QualityAreaId.ToString();

    public int QualityAreaId { get; set; }
    public int Order { get; set; }
    public required string Code { get; set; }
    public required string Description { get; set; }
  }
}
