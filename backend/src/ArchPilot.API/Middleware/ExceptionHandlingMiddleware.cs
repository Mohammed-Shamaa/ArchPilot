using System.Net;
using System.Text.Json;
using ArchPilot.Application.Common.Models;
using FluentValidation;

namespace ArchPilot.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            ValidationException ex => (HttpStatusCode.BadRequest, string.Join(", ", ex.Errors.Select(e => e.ErrorMessage))),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized"),
            KeyNotFoundException => (HttpStatusCode.NotFound, "Resource not found"),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var result = ApiResult<object>.Failure(message);
        var json = JsonSerializer.Serialize(result, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        await context.Response.WriteAsync(json);
    }
}