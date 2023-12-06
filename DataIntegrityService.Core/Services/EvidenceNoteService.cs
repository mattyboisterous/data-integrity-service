using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Interfaces;
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

    public void Execute()
    {
      Console.WriteLine($"EvidenceNoteService doing some work...");
    }
  }
}
