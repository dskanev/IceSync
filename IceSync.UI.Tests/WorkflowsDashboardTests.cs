using Bunit;
using NUnit.Framework;
using Moq;
using IceSync.UI.Services;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using IceSync.UI.Models;
using IceSync.UI.Pages;
using Microsoft.AspNetCore.Components.Web;

namespace IceSync.UI.Tests
{
    [TestFixture]
    public class WorkflowsDashboardTests
    {
        private Bunit.TestContext _testContext;
        private Mock<IWorkflowService> _mockWorkflowService;

        [SetUp]
        public void Setup()
        {
            _testContext = new Bunit.TestContext();

            _mockWorkflowService = new Mock<IWorkflowService>();
            _mockWorkflowService.Setup(s => s.GetAllWorkflowsAsync())
                .ReturnsAsync(new List<WorkflowOutputModel>
                {
                new WorkflowOutputModel(1, "TestWorkflow", false, false, "Test")
                });

            _testContext.Services.AddSingleton(_mockWorkflowService.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _testContext?.Dispose();
        }

        [Test]
        public void WorkflowsDashboard_ShouldRenderLoadingState()
        {
            _mockWorkflowService.Setup(s => s.GetAllWorkflowsAsync()).ReturnsAsync(value: null);

            var cut = _testContext.RenderComponent<WorkflowsDashboard>();
            Assert.IsTrue(cut.Markup.Contains("Loading..."));
        }

        [Test]
        public async Task WorkflowsDashboard_ShouldRunWorkflowAndDisplayNotification()
        {
            _mockWorkflowService.Setup(s => s.RunWorkflowAsync(It.IsAny<int>())).ReturnsAsync(true);

            var cut = _testContext.RenderComponent<WorkflowsDashboard>();

            var button = cut.Find(".run-btn");
            await button.ClickAsync(new MouseEventArgs());

            var notification = cut.Find(".success");
            Assert.IsNotNull(notification);
            Assert.AreEqual("Successfully run!", notification.TextContent);
        }

        [Test]
        public async Task WorkflowsDashboard_ShouldRemoveNotificationAfterThreeSeconds()
        {
            _mockWorkflowService.Setup(s => s.RunWorkflowAsync(It.IsAny<int>())).ReturnsAsync(true);

            var cut = _testContext.RenderComponent<WorkflowsDashboard>();

            var button = cut.Find(".run-btn");
            await button.ClickAsync(new MouseEventArgs());

            await Task.Delay(3100);

            cut.Render();

            var notification = cut.FindAll(".comic-bubble");
            Assert.AreEqual(0, notification.Count);
        }
    }
}