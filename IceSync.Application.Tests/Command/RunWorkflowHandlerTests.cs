using IceSync.Application.Workflows.Commands.Run;
using IceSync.Infrastructure.Workflows;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceSync.Application.Tests.Command
{
    [TestFixture]
    public class RunWorkflowHandlerTests
    {
        private Mock<IUniLoaderApi> _uniLoaderApiMock;
        private RunWorkflowHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _uniLoaderApiMock = new Mock<IUniLoaderApi>();
            _handler = new RunWorkflowHandler(_uniLoaderApiMock.Object);
        }

        [Test]
        public async Task Handle_ShouldCallRunWorkflow_WithCorrectId()
        {
            // Arrange
            var workflowId = 123;
            var command = new RunWorkflowCommand(workflowId);

            _uniLoaderApiMock.Setup(api => api.RunWorkflow(workflowId))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            await _handler.Handle(command, default);

            // Assert
            _uniLoaderApiMock.Verify();
        }

        [Test]
        public void Handle_WhenApiThrowsException_ShouldPropagateException()
        {
            // Arrange
            var workflowId = 123;
            var command = new RunWorkflowCommand(workflowId);
            _uniLoaderApiMock.Setup(api => api.RunWorkflow(workflowId))
                .ThrowsAsync(new InvalidOperationException("Test exception"));

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, default));
        }
    }
}
