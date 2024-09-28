using Microsoft.AspNetCore.Authentication.Cookies;

namespace Configurations
{
    public static class IdentityConfiguration
    {

        public static WebApplicationBuilder AddIdentityConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LogoutPath = "/login";
                    options.AccessDeniedPath = "/access-denied";
                });

            return builder;
        }

        public static WebApplication UseIdentityConfiguration(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
