using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Core.ApiConfigurations
{
    public static class CorsConfiguration
    {
        public static WebApplicationBuilder AddCorsConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Staging", options =>
                {
                    options
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });

                options.AddPolicy("Development", options =>
                {
                    options
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });

                options.AddPolicy("Production", options =>
                {
                    options
                        .WithOrigins("https://localhost")
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            return builder;
        }
    }
}
