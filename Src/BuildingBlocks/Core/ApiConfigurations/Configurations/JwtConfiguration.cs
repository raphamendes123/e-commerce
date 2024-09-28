using Core.Security.Jwt.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Core.Jwt;

namespace Core.ApiConfigurations
{
    public static class JwtConfiguration
    {
        public static WebApplicationBuilder AddJwtConfiguration(this WebApplicationBuilder builder)
        {
            var jwtSettingSelection = builder.Configuration.GetSection("JwtSettings");
            builder.Services.Configure<JwtSettings>(jwtSettingSelection);

            var jwtSettings = jwtSettingSelection.Get<JwtSettings>();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

            builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),// 
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidIssuer = jwtSettings.Issuer,
                };
            });

            return builder;
        }

        public static WebApplicationBuilder AddJwksConfiguration(this WebApplicationBuilder builder)
        {
            var jwksSettingSelection = builder.Configuration.GetSection("JwksSettings");
            builder.Services.Configure<JwksSettings>(jwksSettingSelection);
            var JwksSettings = jwksSettingSelection.Get<JwksSettings>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;//MUDAR ISSO DEPOIS
                
                //SOMENTE PARA gRPC
                options.BackchannelHttpHandler = new HttpClientHandler
                { 
                    ServerCertificateCustomValidationCallback = delegate { return true; }  
                };
                options.SaveToken = true;
                options.SetJwksOptions(
                new JwkOptions(
                    jwksUri: JwksSettings.AuthenticationJwksUrl,
                    issuer: JwksSettings.Issuer,
                    audience: JwksSettings.Audience
                    ));
            });

            builder.Services.AddAuthorization();

            return builder;
        }
    }
}