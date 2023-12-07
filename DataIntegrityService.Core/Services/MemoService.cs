using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Providers
{
  public class MemoService : IDataService
  {
    public string Key => "Memo";
    public required DataServiceConfiguration Settings { get; set; }

    public void Initialise()
    {
      throw new NotImplementedException();
    }
  }
}
