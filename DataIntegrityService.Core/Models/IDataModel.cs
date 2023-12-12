using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Models
{
  public interface IDataModel
  {
    string Key { get; }
  }
}
