using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Workhub.Api.Middleware;

/// <summary>
/// Global exception handling middleware that catches all unhandled exceptions
/// and returns consistent error responses using ProblemDetails format
/// </summary>
public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";

        var problemDetails = exception switch
        {
            Workhub.Domain.Exceptions.ValidationException validationEx => CreateValidationProblemDetails(
                context,
                validationEx),

            Workhub.Domain.Exceptions.NotFoundException notFoundEx => CreateProblemDetails(
                context,
                HttpStatusCode.NotFound,
                "Resource Not Found",
                notFoundEx.Message,
                notFoundEx),

            Workhub.Domain.Exceptions.DuplicateException duplicateEx => CreateProblemDetails(
                context,
                HttpStatusCode.Conflict,
                "Duplicate Resource",
                duplicateEx.Message,
                duplicateEx),

            Workhub.Domain.Exceptions.BusinessRuleViolationException businessEx => CreateProblemDetails(
                context,
                HttpStatusCode.BadRequest,
                "Business Rule Violation",
                businessEx.Message,
                businessEx),

            ArgumentNullException argEx => CreateProblemDetails(
                context,
                HttpStatusCode.BadRequest,
                "Invalid Request",
                argEx.Message,
                argEx),

            ArgumentException argEx => CreateProblemDetails(
                context,
                HttpStatusCode.BadRequest,
                "Invalid Request",
                argEx.Message,
                argEx),

            UnauthorizedAccessException => CreateProblemDetails(
                context,
                HttpStatusCode.Unauthorized,
                "Unauthorized",
                "You are not authorized to access this resource.",
                exception),

            KeyNotFoundException => CreateProblemDetails(
                context,
                HttpStatusCode.NotFound,
                "Resource Not Found",
                "The requested resource was not found.",
                exception),

            InvalidOperationException invOpEx => CreateProblemDetails(
                context,
                HttpStatusCode.BadRequest,
                "Invalid Operation",
                invOpEx.Message,
                invOpEx),

            _ => CreateProblemDetails(
                context,
                HttpStatusCode.InternalServerError,
                "Internal Server Error",
                "An unexpected error occurred. Please try again later.",
                exception)
        };

        context.Response.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, options));
    }

    private ValidationProblemDetails CreateValidationProblemDetails(
        HttpContext context,
        Workhub.Domain.Exceptions.ValidationException validationException)
    {
        var problemDetails = new ValidationProblemDetails(validationException.Errors)
        {
            Status = (int)HttpStatusCode.BadRequest,
            Title = "Validation Error",
            Detail = validationException.Message,
            Instance = context.Request.Path,
            Type = "https://httpstatuses.com/400"
        };

        problemDetails.Extensions["traceId"] = context.TraceIdentifier;

        return problemDetails;
    }

    private ProblemDetails CreateProblemDetails(
        HttpContext context,
        HttpStatusCode statusCode,
        string title,
        string detail,
        Exception exception)
    {
        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path,
            Type = $"https://httpstatuses.com/{(int)statusCode}"
        };

        // In development, include exception details
        if (_environment.IsDevelopment())
        {
            problemDetails.Extensions["exception"] = exception.GetType().Name;
            problemDetails.Extensions["stackTrace"] = exception.StackTrace;
            
            if (exception.InnerException != null)
            {
                problemDetails.Extensions["innerException"] = exception.InnerException.Message;
            }
        }

        // Add trace identifier for debugging
        problemDetails.Extensions["traceId"] = context.TraceIdentifier;

        return problemDetails;
    }
}
