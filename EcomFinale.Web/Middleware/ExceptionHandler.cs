using System.Net;
using System.Text.Json;

namespace EcomFinale.Web.Middleware;

public class ApiExceptionResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string? Details { get; set; } // For stack traces (Development mode only)

    public ApiExceptionResponse(int statusCode, string message, string? details = null)
    {
        StatusCode = statusCode;
        Message = message;
        Details = details;
    }
}

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Pass the request to the next middleware in the pipeline
            await _next(context);
        }
        catch (Exception ex)
        {
            // If any exception happens anywhere below, catch it here
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        // 1. Determine the status code based on exception type
        var statusCode = exception switch
        {
            KeyNotFoundException => HttpStatusCode.NotFound,        // 404
            UnauthorizedAccessException => HttpStatusCode.Unauthorized, // 401
            ArgumentException => HttpStatusCode.BadRequest,          // 400
            _ => HttpStatusCode.InternalServerError                  // 500 (Default)
        };

        context.Response.StatusCode = (int)statusCode;

        // 2. Build the response payload
        // Only show the full stack trace if we are running in Development mode
        var response = _env.IsDevelopment()
            ? new ApiExceptionResponse(context.Response.StatusCode, exception.Message, exception.StackTrace)
            : new ApiExceptionResponse(context.Response.StatusCode, "An internal server error occurred.");

        // 3. Serialize to camelCase (standard for Angular frontend)
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(response, options);

        await context.Response.WriteAsync(json);
    }
}