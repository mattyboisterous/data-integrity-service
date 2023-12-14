﻿using DataIntegrityService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Services.Local
{
  public interface ILocalCacheService
  {
    IEnumerable<IDataModel> GetAllLocal(string key);
    void RemoveIfExists(string key);
    void Insert(string key, IEnumerable<IDataModel> data);
  }
}
