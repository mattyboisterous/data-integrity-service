using DataIntegrityService.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Mappers
{
  public class PassThroughMapper<T1, T2> : IModelMapper<T1, T2>
  {
    public string Key => "PassThrough";

    public T2 MapDataModel(T1 model)
    {
      if (typeof(T1) == typeof(T2))
      {
        // if they are the same type, no transformation needed...
        return (T2)(object)model;
      }

      throw new NotSupportedException("PassThroughMapper assumes T1 and T2 are the same type.");
    }

    public List<T2> MapDataModels(List<T1> model)
    {
      if (typeof(T1) == typeof(T2))
      {
        // if they are the same type, no transformation needed...
        return (List<T2>)(object)model;
      }

      throw new NotSupportedException("PassThroughMapper assumes T1 and T2 are the same type.");
    }
  }
}
