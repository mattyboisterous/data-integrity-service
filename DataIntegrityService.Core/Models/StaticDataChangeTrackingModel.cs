using DataIntegrityService.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Models
{
  public class StaticDataChangeTrackingModel : IDataModel
  {
    public string Key => "ReferenceData";

    public required string Id { get; set; }
    public required string DatasetName { get; set; }
    public string? Version { get; set; }
    public IEnumerable<PropertyBagItem>? PropertyBag { get; set; }
  }

  public class PropertyBagItem
  {
    public required string Key { get; set; }
    public required string Value { get; set; }
  }
}
