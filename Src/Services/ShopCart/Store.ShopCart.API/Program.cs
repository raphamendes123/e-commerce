using Configurations;
using System.Net.NetworkInformation;
using System.Reflection;
using MediatR;
using Core.ApiConfigurations;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Store.ShopCart.API.Services.gRPC;


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
 
//GRPC
builder.Services.AddGrpc();

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

app.MapGrpcService<CartGrpcService>().RequireCors();


app.Run();
