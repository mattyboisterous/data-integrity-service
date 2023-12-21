using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Models
{
  public class DataChangeTrackingModel : IDataModel
  {
    public string Key => Id;

    public required string Id { get; set; }
    public required string UserId { get; set; }
    public required string ItemKey { get; set; }
    public required string DatasetName { get; set; }
    public required string Action { get; set; }
    public string? Version { get; set; }
    public required int Attempts { get; set; }
    public DateTime LastAttempt { get; set; }
    public required DateTime Created { get; set; }
    public IEnumerable<PropertyBagItem>? PropertyBag { get; set; }
  }

  public class PropertyBagItem
  {
    public required string Key { get; set; }
    public required string Value { get; set; }
  }
}
