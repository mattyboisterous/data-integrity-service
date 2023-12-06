using DataIntegrityService.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Interfaces
{
  public interface IDataService
  {
    string Key { get; }
    DataServiceConfiguration Settings { get; set; }
  }
}
