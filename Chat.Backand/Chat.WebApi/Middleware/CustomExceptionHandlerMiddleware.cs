using System.Net;
using FluentValidation;
using Chat.Application.Common.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Chat.WebApi.Middleware;

public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public CustomExceptionHandlerMiddleware(RequestDelegate next) =>
        _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = exception switch
        {
            ValidationException => HttpStatusCode.BadRequest,
            NotFoundException => HttpStatusCode.Forbidden,
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            _ => HttpStatusCode.InternalServerError,
        };

        var apiError = exception switch
        {
            ValidationException validationException => new ApiError
                {
                    ErrorsValidation = validationException.Errors
                                        .Select(x => new ErrorsValidation
                                        {
                                            PropertyName = x.PropertyName,
                                            ErrorMessage = x.ErrorMessage
                                        })
                                        .ToArray()
                },
            _ => new ApiError { Message = exception.Message },
        };

        var result = JsonConvert.SerializeObject(apiError, JsonSettings.Api);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        return context.Response.WriteAsync(result);
    }

    private class ApiError
    {
        public string? Message { get; set; }
        public ErrorsValidation[]? ErrorsValidation { get; set; }
    }

    private class ErrorsValidation
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
    }

    public static class JsonSettings
    {
        public static JsonSerializerSettings Api
        {
            get
            {
                var settings = new JsonSerializerSettings();
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                return settings;
            }
        }
    }
}
