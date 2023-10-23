using IceSync.API.Controllers;
using IceSync.Application.Workflows.Commands.Run;
using IceSync.Application.Workflows.Queries.GetAll;
using IceSync.Infrastructure.Models.Output;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceSync.API.Tests
{
    public class WorkflowsControllerTests
    {
        private Mock<IMediator> mediatorMock;
        private WorkflowsController controller;

        [SetUp]
        public void SetUp()
        {
            mediatorMock = new Mock<IMediator>();
            controller = new WorkflowsController(mediatorMock.Object);
        }

        [Test]
        public async Task Get_ShouldSend_GetAllWorkflowsQuery()
        {
            // Arrange
            var mockResponse = new List<WorkflowOutputModel>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetAllWorkflowsQuery>(), default)).ReturnsAsync(mockResponse);

            // Act
            var result = await controller.Get() as OkObjectResult;

            // Assert
            mediatorMock.Verify(m => m.Send(It.IsAny<GetAllWorkflowsQuery>(), default), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(mockResponse, result.Value);
        }

        [Test]
        public async Task Run_ShouldSend_RunWorkflowCommand()
        {
            // Arrange
            var workflowId = 1;
            mediatorMock.Setup(m => m.Send(It.IsAny<RunWorkflowCommand>(), default)).Returns(Task.CompletedTask);

            // Act
            await controller.Run(workflowId);

            // Assert
            mediatorMock.Verify(m => m.Send(It.Is<RunWorkflowCommand>(cmd => cmd.WorkflowId == workflowId), default), Times.Once);
        }
    }
}
