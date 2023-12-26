using DataIntegrityService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Services.Interfaces
{
  public interface IStaticDataService
  {
    public List<StaticDataChangeTrackingModel> LocalReferenceDataSetState { get; set; }
    public List<StaticDataChangeTrackingModel> ServerReferenceDataSetState { get; set; }
  }
}
