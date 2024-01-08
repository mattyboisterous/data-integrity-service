using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataIntegrityService.Core.Models.Interfaces;

namespace DataIntegrityService.Console.Models
{
    public class ProvisionModel : IDataModel
  {
    public string Key => ProvisionId.ToString();

    public int ProvisionId { get; set; }
  }
}
