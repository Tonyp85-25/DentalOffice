using System.Net;
using System.Text.Json;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CleanTeeth.API.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IProblemDetailsService problemDetails)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            await HandleException(context, e);
        }
    }

    private async Task HandleException(HttpContext context, Exception exception)
    {
        
        _logger.LogError(exception, "Unhandled exception. TraceId: {TraceId}", context.TraceIdentifier);
        

        context.Response.ContentType = "application/json";
        var result = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Server error",
            Type = exception.GetType().Name,
            Detail = context.RequestServices
                .GetRequiredService<IHostEnvironment>()
                .IsDevelopment()
                ? exception.Message
                : null,
            Instance = context.Request.Path
        };

        switch (exception)
        {
            case NotFoundException:
                result.Status =StatusCodes.Status404NotFound;
                result.Title = "Resource Not Found";
                break;
            case CustomValidationException customValidationException:
                
                result = new ValidationProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid request",
                    Detail = customValidationException.Message,
                    Instance = context.Request.Path,
                    Errors = customValidationException.ToDictionary()
                };
                
                break;
            case BusinessRuleException businessRuleException:
                result.Status = StatusCodes.Status400BadRequest;
                result.Title = "Invalid request";
                result.Detail = businessRuleException.Message;
                break;
            case AlreadyExistsException:
               result.Status = StatusCodes.Status409Conflict;
               result.Title = "Resource already exists";
                break;
        }
        context.Response.StatusCode = result.Status ?? StatusCodes.Status500InternalServerError;
        result.Extensions["traceId"] = context.TraceIdentifier;
        result.Extensions["timestamp"] = DateTime.UtcNow;

        await context.Response.WriteAsJsonAsync(result);
    }
    
 
    
}

public static class ErrorHandlingMiddleWareExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}