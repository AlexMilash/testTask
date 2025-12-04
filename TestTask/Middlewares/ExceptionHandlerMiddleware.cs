using System.Net;
using System.Text.Json;
using TestTask.PostingsClient.Contracts.Exceptions;

namespace TestTask.Middlewares
{
    public class TestExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TestExceptionHandlerMiddleware> _logger;

        public TestExceptionHandlerMiddleware(RequestDelegate next, ILogger<TestExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // a pretty dummy exception handling example
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Call the next middleware
            }
            catch (BaseDomainException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode code)
        {
            context.Response.ContentType = "application/json";

            // You can customize status code depending on exception type
            context.Response.StatusCode = (int)code;

            var response = new
            {
                message = exception.Message,
                // optional: stackTrace = exception.StackTrace
            };

            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }
    }
}
