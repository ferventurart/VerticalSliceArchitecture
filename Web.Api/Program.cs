using System.Security.Cryptography;
using Web.Api;
using Web.Api.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddApiServices()
       .AddDatabase()
       .AddApplicationServices()
       .AddErrorHandling();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    await app.ApplyMigrationsAsync();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.MapEndpoints();

await app.RunAsync();
