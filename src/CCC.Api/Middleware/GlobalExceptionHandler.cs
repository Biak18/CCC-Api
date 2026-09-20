using CCC.Application.Common;
using CCC.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CCC.Api.Middleware;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(
            exception,
            "Unhandled exception for {Path}",
            httpContext.Request.Path);

        var problemDetails = Map(exception);

        httpContext.Response.StatusCode =
            problemDetails.Status ?? StatusCodes.Status500InternalServerError;

        return await problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = problemDetails
            });
    }


    private static ProblemDetails Map(Exception exception)
    {
        switch (exception)
        {
            case ValidationException validation:
                {
                    var problem = new ProblemDetails
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "One or more validation errors occurred."
                    };

                    problem.Extensions["errors"] = validation.Errors
                        .GroupBy(x => x.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(x => x.ErrorMessage).ToArray());

                    return problem;
                }

            case NotFoundException:
                return new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Resource not found.",
                    Detail = exception.Message
                };

            case ConflictException:
                return new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Conflict.",
                    Detail = exception.Message
                };

            case ForbiddenException:
                return new ProblemDetails
                {
                    Status = StatusCodes.Status403Forbidden,
                    Title = "Forbidden.",
                    Detail = exception.Message
                };

            case UnauthorizedAccessException:
                return new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "Unauthorized."
                };

            case DomainException:
                return new ProblemDetails
                {
                    Status = StatusCodes.Status422UnprocessableEntity,
                    Title = "The operation is not allowed.",
                    Detail = exception.Message
                };

            default:
                return new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "An unexpected error occurred."
                };
        }
    }
}
