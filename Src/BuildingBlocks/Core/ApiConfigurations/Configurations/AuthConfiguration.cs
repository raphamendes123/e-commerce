using Microsoft.AspNetCore.Builder;

namespace Core.ApiConfigurations
{
    public static class AuthConfiguration
    {
        public static IApplicationBuilder UseAuthConfiguration(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentException(nameof(app));

            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
