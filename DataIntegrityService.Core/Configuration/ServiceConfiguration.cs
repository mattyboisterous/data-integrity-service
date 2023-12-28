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
    public required string ReferenceDataService { get; set; }
    public required ChangeTrackingConfiguration ChangeTrackingService { get; set; }
    public required List<DataServiceConfiguration> DataServices { get; set; }
  }

  public sealed class ChangeTrackingConfiguration
  {
    public required bool posionMessages { get; set; }
    public List<int> BackOff { get; set; } = [];
    public required HttpServiceConfiguration Http { get; set; }
    public required LocalCacheServiceConfiguration Cache { get; set; }
  }

  public sealed class DataServiceConfiguration
  {
    public required string Key { get; set; }
    public required string DatasetName { get; set; }
    public required string DatasetGroup { get; set; }
    public WorkflowConfiguration? Push { get; set; }
    public WorkflowConfiguration? Pull { get; set; }
    public required HttpServiceConfiguration Http { get; set; }
    public required LocalCacheServiceConfiguration Cache { get; set; }
  }

  public sealed class WorkflowConfiguration
  {
    public required string DataWorkflow { get; set; }
    public required List<string> Dependencies { get; set; }
  }

  public sealed class HttpServiceConfiguration
  {
    public string? Get { get; set; }
    public string? GetAllByKey { get; set; }
    public string? Put { get; set; }
    public string? Post { get; set; }
    public string? Delete { get; set; }
  }

  public sealed class LocalCacheServiceConfiguration
  {
    public string? Key { get; set; }
    public string? AltKey { get; set; }
    public string? KeyMap { get; set; }
  }
}
