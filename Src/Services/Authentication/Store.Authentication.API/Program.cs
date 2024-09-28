using Core.ApiConfigurations;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Configurations;
using Core.Security.AspNetCore; 

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
    .AddDbContextConfiguration()
    .AddDependencyInjectionConfiguration()
    .AddJwksConfiguration()
    .AddMessageQueueConfiguration();

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

app.UseJwksDiscovery();

app.Run();
