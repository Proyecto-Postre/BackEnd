using System.Net;
using System.Text.Json;

namespace DulceFe.API.Shared.Infrastructure.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
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
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        // Default to Internal Server Error
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        var message = "An unexpected error occurred.";

        // Handle specific exceptions if necessary (e.g., custom domain exceptions)
        if (exception is KeyNotFoundException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            message = exception.Message;
        }
        else if (exception is ArgumentException || exception is InvalidOperationException) // Simplification for domain rules
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            message = exception.Message;
        }

        // Log the full error
        _logger.LogError(exception, $"Error occurred: {exception.Message}");

        var result = JsonSerializer.Serialize(new { message = message, detail = exception.Message }); // Detail only for dev/v1, usually hidden in Prod
        return context.Response.WriteAsync(result);
    }
}
