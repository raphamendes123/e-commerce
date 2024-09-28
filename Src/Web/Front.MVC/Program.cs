using Core.ApiConfigurations;
using Configurations;
using Front.MVC.Middlewares;
using Microsoft.AspNetCore.HttpOverrides;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);


builder
   .AddMvcConfiguration()
   .AddIdentityConfiguration()
   .AddDependencyInjectionConfig();

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Services.Configure<AppSettings>(builder.Configuration);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});


WebApplication? app = builder.Build();

app.UseForwardedHeaders();

app
    .UseMvcConfiguration()
    .UseIdentityConfiguration()
    .UseMiddleware<ExpectionMiddleware>();

app.UseCultureInfoConfiguration("pt-BR");

app.Run();
