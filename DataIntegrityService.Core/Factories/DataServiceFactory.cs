using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Factories
{
    public class DataServiceFactory
    {
        private readonly IEnumerable<IDataService> _dataServices;

        public DataServiceFactory(IEnumerable<IDataService> dataServices)
        {
            _dataServices = dataServices;
        }
        public IDataService GetDataService(DataServiceConfiguration serviceConfiguration)
        {
            var dataService = _dataServices.FirstOrDefault(ds => ds.Key == serviceConfiguration.Key)
                        ?? throw new NotSupportedException($"Data Service '{serviceConfiguration.Key}' has not been configured to run.");

            dataService.Settings = serviceConfiguration;

            return dataService;
        }
    }
}
