using IceSync.Infrastructure.Models.Output;
using IceSync.Infrastructure.Services;
using IceSync.Infrastructure.Workflows;
using IceSync.Persistance.Models;
using IceSync.Persistance.Repositories;
using Moq;

namespace IceSync.Infrastructure.Tests.Services
{
    [TestFixture]
    public class WorkflowServiceTests
    {
        private Mock<IWorkflowRepository> _workflowRepositoryMock;
        private Mock<IUniLoaderApi> _uniLoaderApiMock;
        private WorkflowService _service;

        [SetUp]
        public void SetUp()
        {
            _workflowRepositoryMock = new Mock<IWorkflowRepository>();
            _uniLoaderApiMock = new Mock<IUniLoaderApi>();
            _service = new WorkflowService(_workflowRepositoryMock.Object, _uniLoaderApiMock.Object);
        }

        [Test]
        public async Task GetAllWorkflows_ShouldReturndWorkflowsFromRepository()
        {
            // Arrange
            var mockWorkflows = new List<Workflow> { new Workflow { } };
            _workflowRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(mockWorkflows);

            // Act
            var result = await _service.GetAllWorkflows();

            // Assert
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public async Task SyncWithApi_ShouldSyncDatabaseWorkflowsWithApiWorkflows()
        {
            // Arrange
            var mockDatabaseWorkflowId = 1;
            var mockApiWorkflow = new WorkflowOutputModel { Id = 2 };

            _workflowRepositoryMock.Setup(repo => repo.GetAllIds()).ReturnsAsync(new List<int> { mockDatabaseWorkflowId });
            _uniLoaderApiMock.Setup(api => api.GetAllWorkflows()).ReturnsAsync(new List<WorkflowOutputModel> { mockApiWorkflow });

            _workflowRepositoryMock.Setup(repo => repo.DeleteManyById(It.IsAny<IList<int>>())).Returns(Task.CompletedTask);
            _workflowRepositoryMock.Setup(repo => repo.InsertMany(It.IsAny<IList<Workflow>>())).Returns(Task.CompletedTask);

            // Act
            await _service.SyncWithApi();

            // Assert
            _workflowRepositoryMock.Verify(repo => repo.DeleteManyById(It.Is<IList<int>>(ids => ids.Contains(mockDatabaseWorkflowId))), Times.Once);
            _workflowRepositoryMock.Verify(repo => repo.InsertMany(It.Is<IList<Workflow>>(workflows => workflows.Any(w => w.Id == mockApiWorkflow.Id))), Times.Once);
        }
    }
}
