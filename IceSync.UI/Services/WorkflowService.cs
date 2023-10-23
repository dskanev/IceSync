using IceSync.UI.Models;
using Refit;

namespace IceSync.UI.Services
{
    public interface IWorkflowService
    {
        Task<IList<WorkflowOutputModel>> GetAllWorkflowsAsync();
        Task<bool> RunWorkflowAsync(int workflowId);
    }

    public class WorkflowService : IWorkflowService
    {
        private readonly IIceSyncApi _api;

        public WorkflowService(IIceSyncApi api)
        {
            _api = api;
        }

        public async Task<IList<WorkflowOutputModel>> GetAllWorkflowsAsync()
        {
            return await _api.GetAllWorkflows();
        }

        public async Task<bool> RunWorkflowAsync(int workflowId)
        {
            try
            {
                await _api.RunWorkflow(workflowId);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
