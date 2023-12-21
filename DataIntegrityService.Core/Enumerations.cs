using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core
{
  public enum DatasetMethod
  {
    Ref,
    Push,
    Pull,
    PushPull,
    PullPush
  }

  public enum ChangeAction
  {
    Create,
    Update,
    Delete
  }
}
