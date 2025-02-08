using WebApi.Middlewares;

namespace WebApi.Extentions;

public static class MiddlewareExtentions
{
    public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}
