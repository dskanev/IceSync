using IceSync.Application.Workflows.Commands.Run;
using IceSync.Application.Workflows.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IceSync.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkflowsController : ControllerBase
    {
        private readonly IMediator mediator;

        public WorkflowsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("/workflows")]
        public async Task<IActionResult> Get()
            => Ok(await mediator.Send(new GetAllWorkflowsQuery()));

        [HttpPost]
        [Route("/workflows/{workflowId}/run")]
        public async Task Run([FromRoute] int workflowId)
            => await mediator.Send(new RunWorkflowCommand(workflowId));
    }
}