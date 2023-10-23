using IceSync.API.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Moq;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IceSync.API.Tests
{
    public class RefitExceptionHandlerMiddlewareTests
    {
        private Mock<RequestDelegate> _nextMock;
        private RefitExceptionHandlerMiddleware _middleware;

        [SetUp]
        public void SetUp()
        {
            _nextMock = new Mock<RequestDelegate>();
            _middleware = new RefitExceptionHandlerMiddleware(_nextMock.Object);
        }

        [Test]
        public async Task Middleware_Processes_Normally_When_No_Exception()
        {
            // Arrange
            var context = new DefaultHttpContext();

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            _nextMock.Verify(next => next(context), Times.Once);
        }

        [Test]
        public async Task Middleware_Catches_NotFoundApiException()
        {
            // Arrange
            var context = new DefaultHttpContext
            {
                Response = { Body = new MemoryStream() }
            };

            var expectedException = await CreateApiException(HttpStatusCode.NotFound, "Reason: NotFound");
            _nextMock.Setup(next => next(It.IsAny<HttpContext>())).Throws(expectedException);

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var responseBody = await reader.ReadToEndAsync();
            var expectedBody = JsonConvert.SerializeObject(new
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = expectedException.ReasonPhrase
            });

            Assert.AreEqual((int)HttpStatusCode.NotFound, context.Response.StatusCode);
            Assert.AreEqual(expectedBody, responseBody);
        }

        private static async Task<ApiException> CreateApiException(HttpStatusCode statusCode, string reasonPhrase)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://test.com");
            var httpResponseMessage = new HttpResponseMessage(statusCode)
            {
                ReasonPhrase = reasonPhrase,
                Content = new StringContent("")
            };
            var refitSettings = new RefitSettings();

            return await ApiException.Create("Custom Exception Message", httpRequestMessage, HttpMethod.Get, httpResponseMessage, refitSettings);
        }
    }
}
