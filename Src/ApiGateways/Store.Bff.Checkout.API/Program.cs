using Configurations;
using Core.ApiConfigurations; 
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

if (builder.Environment.IsProduction())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder
    .AddApiConfiguration()
    .AddCorsConfiguration()
    .AddSwaggerConfiguration() 
    .AddDependencyInjectionConfiguration()
    .AddJwksConfiguration()
    .AddMessageQueueConfiguration()
    .AddGrpcConfiguration();

builder.Services.Configure<AppSettings>(builder.Configuration);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

WebApplication app = builder.Build();

IApiVersionDescriptionProvider provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

if (app.Environment.IsDevelopment())
{
    app.UseCors("Development");
}
else
{
    app.UseCors("Production");
    app.UseHsts();
}

app.UseSwaggerConfiguration(provider);

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthConfiguration();

app.UseCultureInfoConfiguration(cultureInfo: "pt-BR");

app.MapControllers();

app.Run();
