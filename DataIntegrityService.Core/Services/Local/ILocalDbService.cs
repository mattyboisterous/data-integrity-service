using DataIntegrityService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Services.Local
{
  public interface ILocalDbService
  {
    int DeleteAll<IDataModel>();
    int InsertAll(IEnumerable<IDataModel> data);
  }
}
