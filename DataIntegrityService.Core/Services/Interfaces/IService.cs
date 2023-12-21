using DataIntegrityService.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Services.Interfaces
{
    public interface IService
    {
        bool IsInitialised { get; set; }
        string Key { get; }
        void Initialise();
    }
}
