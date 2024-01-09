using DataIntegrityService.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Console.Models
{
  public class MemoModel : IDataModel
  {
    public string Key => MemoId.ToString();

    public int MemoId { get; set; }
  }
}
