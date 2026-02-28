using System.Net;
using System.Text.Json;
using FluentValidation;
using ShopxBase.Domain.Exceptions;

namespace ShopxBase.Api.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, errorCode, message, errors) = exception switch
        {
            // FluentValidation
            ValidationException validationEx => (
                HttpStatusCode.BadRequest,
                "VALIDATION_ERROR",
                "Dữ liệu không hợp lệ",
                validationEx.Errors.Select(e => e.ErrorMessage).ToArray()
            ),

            // Auth exceptions
            UnauthorizedUserException unauthorizedEx => (
                HttpStatusCode.Unauthorized,
                unauthorizedEx.Code,
                unauthorizedEx.Message,
                new[] { unauthorizedEx.Message }
            ),

            ForbiddenAccessException forbiddenEx => (
                HttpStatusCode.Forbidden,
                forbiddenEx.Code,
                forbiddenEx.Message,
                new[] { forbiddenEx.Message }
            ),

            // Not found exceptions
            ShopNotFoundException or
            CategoryNotFoundException or
            BrandNotFoundException or
            UserNotFoundException or
            OrderNotFoundException or
            BusinessRegistrationNotFoundException => (
                HttpStatusCode.NotFound,
                ((DomainException)exception).Code,
                exception.Message,
                new[] { exception.Message }
            ),

            // Business rule violations
            DuplicateRegistrationException or
            DuplicateShopSlugException or
            UserAlreadyExistsException => (
                HttpStatusCode.Conflict,
                ((DomainException)exception).Code,
                exception.Message,
                new[] { exception.Message }
            ),

            InsufficientStockException insufficientEx => (
                HttpStatusCode.BadRequest,
                insufficientEx.Code,
                insufficientEx.Message,
                new[] { insufficientEx.Message }
            ),

            InvalidCouponException couponEx => (
                HttpStatusCode.BadRequest,
                couponEx.Code,
                couponEx.Message,
                new[] { couponEx.Message }
            ),

            // Generic domain exceptions (catch-all for DomainException subtypes)
            DomainException domainEx => (
                HttpStatusCode.BadRequest,
                domainEx.Code,
                domainEx.Message,
                new[] { domainEx.Message }
            ),

            // Unhandled exceptions
            _ => (
                HttpStatusCode.InternalServerError,
                "INTERNAL_ERROR",
                "Đã xảy ra lỗi hệ thống",
                new[] { "Đã xảy ra lỗi hệ thống. Vui lòng thử lại sau." }
            )
        };

        // Log error (full detail for 500, warning for others)
        if (statusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
        }
        else
        {
            _logger.LogWarning("Domain exception [{Code}]: {Message}", errorCode, message);
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            success = false,
            message,
            code = errorCode,
            errors
        };

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
