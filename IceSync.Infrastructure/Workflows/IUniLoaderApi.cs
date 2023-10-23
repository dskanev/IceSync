using IceSync.Infrastructure.Authentication;
using IceSync.Infrastructure.Models.Output;
using Refit;

namespace IceSync.Infrastructure.Workflows
{
    public interface IUniLoaderApi
    {        
        [Get("/workflows")]
        Task<IList<WorkflowOutputModel>> GetAllWorkflows();

        [Post("/workflows/{workflowId}/run")]
        Task RunWorkflow(int workflowId);
    }
}
