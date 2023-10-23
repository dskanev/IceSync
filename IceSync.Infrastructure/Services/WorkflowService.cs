using IceSync.Infrastructure.Models.Output;
using IceSync.Infrastructure.Workflows;
using IceSync.Persistance.Models;
using IceSync.Persistance.Repositories;

namespace IceSync.Infrastructure.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly IWorkflowRepository _workflowRepository;
        private readonly IUniLoaderApi _uniLoaderApi;

        public WorkflowService(IWorkflowRepository workflowRepository,
            IUniLoaderApi uniLoaderApi)
        {
            _workflowRepository = workflowRepository;
            _uniLoaderApi = uniLoaderApi;
        }

        public async Task<IList<WorkflowOutputModel>> GetAllWorkflows()
        {
            var workflows = (await _workflowRepository
                .GetAll())
                .Select(x => new WorkflowOutputModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive,
                    IsRunning = x.IsRunning,
                    MultiExecBehavior = x.MultiExecBehavior
                })
                .ToList();

            return workflows;
        }

        public async Task SyncWithApi()
        {
            var existingWorkflows = await _uniLoaderApi.GetAllWorkflows();
            var databaseWorkflows = await _workflowRepository.GetAllIds();

            var newIds = existingWorkflows.Select(w => w.Id).Except(databaseWorkflows).ToList();
            var oldIds = databaseWorkflows.Except(existingWorkflows.Select(w => w.Id)).ToList();

            var newWorkflows = existingWorkflows.Where(w => newIds.Contains(w.Id)).ToList();
            var noLongerExistingWorkflows = databaseWorkflows.Where(w => oldIds.Contains(w)).ToList();

            await _workflowRepository.DeleteManyById(noLongerExistingWorkflows);
            await _workflowRepository.InsertMany(newWorkflows.Select(x => new Workflow()
            {
                Id = x.Id,
                Name = x.Name,
                IsActive = x.IsActive,
                IsRunning = x.IsRunning,
                MultiExecBehavior = x.MultiExecBehavior
            }).ToList());
        }
    }
}
