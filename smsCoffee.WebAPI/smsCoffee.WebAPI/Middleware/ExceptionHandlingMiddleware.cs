using smsCoffee.WebAPI.Services.CommonService;
using System;
using System.ComponentModel.DataAnnotations;

namespace smsCoffee.WebAPI.Middleware
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
         }
        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var response = ResponseFactory.Error<object>(
                context.Request.Path,
                GetErrorMessage(ex),
                GetStatusCode(ex)
            );

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.StatusCode;

            await context.Response.WriteAsJsonAsync(response);
        }
        private static int GetStatusCode(Exception exception) =>
            exception switch
            {
                ValidationException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                ForbiddenException => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };

        private static string GetErrorMessage(Exception exception) =>
            exception switch
            {
                ValidationException => "Validation failed",
                NotFoundException => "Resource not found",
                UnauthorizedAccessException => "Unauthorized access",
                ForbiddenException => "Access denied",
                _ => "An error occurred while processing your request"
            };
    }
}
// 2. Custom Exception Types
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

public class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base(message) { }
}