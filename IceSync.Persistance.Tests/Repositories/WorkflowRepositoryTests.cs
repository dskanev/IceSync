using IceSync.Persistance.Database;
using IceSync.Persistance.Models;
using IceSync.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceSync.Persistance.Tests.Repositories
{
    [TestFixture]
    public class WorkflowRepositoryTests
    {
        private IceSyncDbContext _dbContext;
        private WorkflowRepository _workflowRepository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<IceSyncDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new IceSyncDbContext(options);
            _workflowRepository = new WorkflowRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
        }

        [Test]
        public async Task GetAll_ShouldReturnAllWorkflows()
        {
            var mockWorkflow = new Workflow { Id = 1, Name = "TestWorkflow" };
            _dbContext.Workflows.Add(mockWorkflow);
            await _dbContext.SaveChangesAsync();

            var result = await _workflowRepository.GetAll();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo("TestWorkflow"));
        }

        [Test]
        public async Task GetAllIds_ShouldReturnAllWorkflowIds()
        {
            var mockWorkflow = new Workflow { Id = 1, Name = "TestWorkflow" };
            _dbContext.Workflows.Add(mockWorkflow);
            await _dbContext.SaveChangesAsync();

            var result = await _workflowRepository.GetAllIds();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(1));
        }

        [Test]
        public void InsertManyWithoutId_ShouldThrowException()
        {
            // Arrange
            var workflow = new Workflow
            {
                Name = "TestWorkflow"
            };

            var workflow2 = new Workflow
            {
                Name = "TestWorkflow2"
            };

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => _workflowRepository.InsertMany(new List<Workflow> { workflow, workflow2 }));
        }

        [Test]
        public async Task InsertMany_ShouldAddMultipleWorkflows()
        {
            var workflows = new List<Workflow>
        {
            new Workflow { Id = 200, Name = "Workflow1" },
            new Workflow { Id = 201, Name = "Workflow2" }
        };

            await _workflowRepository.InsertMany(workflows);

            var result = await _dbContext.Workflows.ToListAsync();

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task DeleteManyById_ShouldDeleteWorkflowsWithGivenIds()
        {
            var mockWorkflow1 = new Workflow { Id = 1, Name = "TestWorkflow1" };
            var mockWorkflow2 = new Workflow { Id = 2, Name = "TestWorkflow2" };
            _dbContext.Workflows.Add(mockWorkflow1);
            _dbContext.Workflows.Add(mockWorkflow2);
            await _dbContext.SaveChangesAsync();

            await _workflowRepository.DeleteManyById(new List<int> { 1 });

            var remainingWorkflows = await _dbContext.Workflows.ToListAsync();

            Assert.That(remainingWorkflows.Count, Is.EqualTo(1));
            Assert.That(remainingWorkflows[0].Name, Is.EqualTo("TestWorkflow2"));
        }
    }

}
