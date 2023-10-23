using IceSync.Application.Workflows.Queries.GetAll;
using IceSync.Infrastructure.Workflows;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceSync.Application.Workflows.Commands.Run
{
    public class RunWorkflowHandler : IRequestHandler<RunWorkflowCommand>
    {
        private readonly IUniLoaderApi _uniLoaderApi;

        public RunWorkflowHandler(IUniLoaderApi uniLoaderApi) 
        {
            _uniLoaderApi = uniLoaderApi;
        }

        public async Task Handle(RunWorkflowCommand request, CancellationToken cancellationToken)
            => await _uniLoaderApi.RunWorkflow(request.WorkflowId);
    }
}
