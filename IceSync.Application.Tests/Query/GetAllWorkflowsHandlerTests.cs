using IceSync.Application.Workflows.Queries.GetAll;
using IceSync.Infrastructure.Models.Output;
using IceSync.Infrastructure.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceSync.Application.Tests.Query
{
    [TestFixture]
    public class GetAllWorkflowsHandlerTests
    {
        private Mock<IWorkflowService> _workflowServiceMock;
        private GetAllWorkflowsHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _workflowServiceMock = new Mock<IWorkflowService>();
            _handler = new GetAllWorkflowsHandler(_workflowServiceMock.Object);
        }

        [Test]
        public async Task Handle_ShouldCallGetAllWorkflows_AndReturnCorrectData()
        {
            // Arrange
            var expectedWorkflows = new List<WorkflowOutputModel>
        {
            new WorkflowOutputModel { },
        };

            _workflowServiceMock.Setup(service => service.GetAllWorkflows())
                .ReturnsAsync(expectedWorkflows);

            var query = new GetAllWorkflowsQuery();

            // Act
            var result = await _handler.Handle(query, default);

            // Assert
            _workflowServiceMock.Verify(service => service.GetAllWorkflows(), Times.Once);

            Assert.AreEqual(expectedWorkflows.Count, result.Count);
        }
    }
}
