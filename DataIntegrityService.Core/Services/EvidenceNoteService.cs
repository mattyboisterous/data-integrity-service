using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Interfaces;
using DataIntegrityService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Providers
{
  public class EvidenceNoteService : IDataService
  {
    public string Key => "EvidenceNote";
    public required DataServiceConfiguration Settings { get; set; }
    public bool IsInitialised { get; set; }

    public void Initialise()
    {
      throw new NotImplementedException();
    }

    public IEnumerable<IDataModel> TransformData(IEnumerable<IDataModel> data)
    {
      throw new NotImplementedException();
    }
  }
}
