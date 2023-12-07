using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Configuration
{
  public sealed class ServiceConfiguration
  {
    public required string Environment { get; set; }
    public required string Version { get; set; }
    public required List<DataServiceConfiguration> DataServices { get; set; }
  }

  public sealed class DataServiceConfiguration
  {
    public required string Key { get; set; }
    public required string DatasetName { get; set; }
    public required string DatasetGroup { get; set; }
    public required string DatasetMethod { get; set; }
    public required ModelTypes Models { get; set; }
    public required List<string> Dependencies { get; set; }
    public required HttpServiceConfiguration Http { get; set; }
  }

  public sealed class ModelTypes
  {
    public required string Source { get; set; }
    public required string Destination { get; set; }
  }

  public sealed class HttpServiceConfiguration
  {
    public string? Get { get; set; }
    public string? Put { get; set; }
    public string? Post { get; set; }
    public string? Delete { get; set; }
  }
}
