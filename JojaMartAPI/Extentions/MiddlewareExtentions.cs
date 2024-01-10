using JojaMartAPI.Extentions.Middleware;

namespace JojaMartAPI.Extentions;

public static class MiddlewareExtentions
{
    public static IApplicationBuilder JwtCustomMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<JwtMiddleware>();
    }
}
