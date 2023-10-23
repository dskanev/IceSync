using IceSync.Infrastructure.Models.Output;
using IceSync.Infrastructure.Services;
using IceSync.Infrastructure.Workflows;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceSync.Application.Workflows.Queries.GetAll
{
    public class GetAllWorkflowsHandler : IRequestHandler<GetAllWorkflowsQuery, IList<WorkflowOutputModel>>
    {
        private readonly IWorkflowService _workflowService;
        public GetAllWorkflowsHandler(IWorkflowService workflowService)
        {
            _workflowService = workflowService;
        }

        public async Task<IList<WorkflowOutputModel>> Handle(
            GetAllWorkflowsQuery request,
            CancellationToken cancellationToken)

        {
            var result = await _workflowService.GetAllWorkflows();
            return result;
        }
    }
}
