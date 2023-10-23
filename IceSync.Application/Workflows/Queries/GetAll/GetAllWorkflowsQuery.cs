using IceSync.Infrastructure.Models.Output;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IceSync.Application.Workflows.Queries.GetAll
{
    public record GetAllWorkflowsQuery() : IRequest<IList<WorkflowOutputModel>>;
}
