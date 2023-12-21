using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Workflows.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Factories
{
    public class WorkflowServiceFactory
    {
        private readonly IEnumerable<IWorkflowService> _dataWorkflows;

        public WorkflowServiceFactory(IEnumerable<IWorkflowService> workflows)
        {
            _dataWorkflows = workflows;
        }
        public IWorkflowService GetDataWorkflow(string key)
        {
            return _dataWorkflows.FirstOrDefault(wf => wf.Key == key)
                        ?? throw new NotSupportedException();
        }
    }
}
