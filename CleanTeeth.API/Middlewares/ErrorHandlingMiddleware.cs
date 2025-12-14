using System.Net;
using System.Text.Json;
using CleanTeeth.Application.Exceptions;

namespace CleanTeeth.API.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
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

    private Task HandleException(HttpContext context, Exception exception)
    {
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

        context.Response.ContentType = "application/json";
        var result = string.Empty;

        switch (exception)
        {
            case NotFoundException:
                statusCode = HttpStatusCode.NotFound;
                break;
            case CustomValidationException customValidationException:
                statusCode = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(customValidationException.ValidationErrors);
                break;
            case AlreadyExistsException:
                statusCode = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize("Data already exists");
                break;
        }

        context.Response.StatusCode = (int)statusCode;
        return context.Response.WriteAsync(result);
    }
    
}

public static class ErrorHandlingMiddleWareExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}