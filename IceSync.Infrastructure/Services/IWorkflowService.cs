using IceSync.Infrastructure.Models.Output;
using IceSync.Infrastructure.Workflows;
using IceSync.Persistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceSync.Infrastructure.Services
{
    public interface IWorkflowService
    {
        Task<IList<WorkflowOutputModel>> GetAllWorkflows();

        Task SyncWithApi();
    }
}
