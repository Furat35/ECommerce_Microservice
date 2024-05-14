using Catalog.API.Extensions;
using Shared.Middleware;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"------------------- {builder.Environment.EnvironmentName}");
Console.WriteLine("--------------------" + builder.Configuration["DatabaseSettings:ConnectionString"]);

builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

app.UseCustomExceptionHandling();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
