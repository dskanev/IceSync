using Newtonsoft.Json;
using Refit;
using System.Net;

namespace IceSync.API.Middleware
{
    public class RefitExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public RefitExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ApiException apiException)
            {
                await HandleRefitExceptionAsync(context, apiException);
            }
        }

        private static Task HandleRefitExceptionAsync(HttpContext context, ApiException exception)
        {
            context.Response.ContentType = "application/json";

            switch (exception.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case HttpStatusCode.Unauthorized:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case HttpStatusCode.Forbidden:
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    break;
                case HttpStatusCode.NotFound:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case HttpStatusCode.InternalServerError:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            return context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.ReasonPhrase
            }));
        }
    }
}
