using System.Text.Json;

namespace MovieTicketBookingSystem.Middleware;

public class GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled exception while processing {Method} {Path}", context.Request.Method, context.Request.Path);
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = exception switch
            {
                KeyNotFoundException => StatusCodes.Status404NotFound,
                ArgumentException => StatusCodes.Status400BadRequest,
                InvalidOperationException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };
            var problem = new
            {
                type = "https://httpstatuses.com/" + context.Response.StatusCode,
                title = context.Response.StatusCode == 500 ? "An unexpected error occurred." : "Request could not be completed.",
                status = context.Response.StatusCode,
                detail = context.Response.StatusCode == 500 ? "The server could not complete the request." : exception.Message,
                traceId = context.TraceIdentifier
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
    }
}
