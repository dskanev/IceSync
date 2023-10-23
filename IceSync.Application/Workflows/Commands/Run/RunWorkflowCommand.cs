using IceSync.Application.Workflows.Queries.GetAll;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceSync.Application.Workflows.Commands.Run
{
    public record RunWorkflowCommand(int WorkflowId) : IRequest;
}
