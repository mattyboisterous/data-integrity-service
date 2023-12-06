using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Services.Local
{
  public interface ILocalDbService
  {
    int DeleteAll<T>();
    int InsertAll<T>(List<T> data);
  }
}
