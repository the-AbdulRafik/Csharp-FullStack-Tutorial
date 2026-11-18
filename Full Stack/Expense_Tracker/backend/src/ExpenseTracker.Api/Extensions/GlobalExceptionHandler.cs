using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Extensions;

public sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var status = exception switch
        {
            ApiNotFoundException => StatusCodes.Status404NotFound,
            ApiConflictException => StatusCodes.Status409Conflict,
            ApiBadRequestException => StatusCodes.Status400BadRequest,
            DbUpdateException => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        if (status >= 500)
            logger.LogError(exception, "Unhandled exception while processing {RequestPath}", httpContext.Request.Path);
        else
            logger.LogWarning(exception, "Request {RequestPath} failed: {Message}", httpContext.Request.Path, exception.Message);

        httpContext.Response.StatusCode = status;
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = status,
                Title = status switch
                {
                    StatusCodes.Status404NotFound => "The requested resource was not found.",
                    StatusCodes.Status409Conflict => "The request conflicts with existing data.",
                    StatusCodes.Status400BadRequest => "The request is invalid.",
                    _ => "An unexpected error occurred."
                },
                Detail = exception.Message
            }
        });
    }
}
