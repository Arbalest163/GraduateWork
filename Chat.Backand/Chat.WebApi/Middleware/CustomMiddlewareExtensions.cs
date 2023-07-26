using Microsoft.AspNetCore.Builder;

namespace Chat.WebApi.Middleware;

public static class CustomMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this
        IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }

    public static IApplicationBuilder UseRequestTimingMiddleware(this
        IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestTimingMiddleware>();
    }

    public static IApplicationBuilder UserCusomCorsMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CorsMiddleware>();
    }
}
