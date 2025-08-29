using System.Net;
using BackendApi.DTOs.Common;
using BackendApi.Exceptions;

namespace BackendApi.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string correlationId = context.TraceIdentifier;
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex, correlationId);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception, string correlationId)
        {
            context.Response.ContentType = "application/json";
            ErrorResponse errorResponse = new ErrorResponse
            {
                CorrelationId = context.TraceIdentifier
            };

            switch (exception)
            {
                case BusinessException bex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = bex.Message;
                    errorResponse.Code = "BUSINESS_ERROR";
                    errorResponse.Details = null;
                    break;
                case ValidationException vex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = vex.Message;
                    errorResponse.Code = "VALIDATION_ERROR";
                    errorResponse.Details = null;
                    break;
                case NotFoundException nfx:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse.Message = nfx.Message;
                    errorResponse.Code = "NOT_FOUND";
                    errorResponse.Details = null;
                    break;
                case ArgumentException aex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = "An error occurred while processing your request.";
                    errorResponse.Code = "ARGUMENT_ERROR";
                    errorResponse.Details = aex.Message;
                    break;
                case KeyNotFoundException kex:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse.Message = "Resource not found.";
                    errorResponse.Code = "KEY_NOT_FOUND";
                    errorResponse.Details = kex.Message;
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Message = "An internal server error occurred.";
                    errorResponse.Code = "INTERNAL_ERROR";
                    errorResponse.Details = null;
                    break;
            }

            var jsonResponse = System.Text.Json.JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
