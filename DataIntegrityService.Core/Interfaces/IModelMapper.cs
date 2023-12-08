using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Interfaces
{
  public interface IModelMapper<T1, T2>
  {
    string Key { get; }
    T2 MapDataModel(T1 model);
    List<T2> MapDataModels(List<T1> model);
  }
}
