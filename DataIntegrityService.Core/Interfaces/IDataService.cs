﻿using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Interfaces
{
  public interface IDataService
  {
    bool IsInitialised { get; set; }
    string Key { get; }
    DataServiceConfiguration Settings { get; set; }

    void Initialise();

    IEnumerable<IDataModel> TransformData(IEnumerable<IDataModel> data);
  }
}
