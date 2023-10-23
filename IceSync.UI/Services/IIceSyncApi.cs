using IceSync.UI.Models;
using Refit;

namespace IceSync.UI.Services
{
    public interface IIceSyncApi
    {
        [Get("/workflows")]
        Task<IList<WorkflowOutputModel>> GetAllWorkflows();

        [Post("/workflows/{workflowId}/run")]
        Task RunWorkflow(int workflowId);
    }
}
