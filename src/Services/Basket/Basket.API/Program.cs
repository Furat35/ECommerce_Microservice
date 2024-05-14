using Basket.API.Extensions;
using Shared.Middleware;

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine($"------------------- {builder.Environment.EnvironmentName}");
Console.WriteLine($"------------------- {builder.Configuration["Catalog.Api"]}");
Console.WriteLine($"------------------- {builder.Configuration["Payment.Api"]}");
Console.WriteLine($"------------------- {builder.Configuration["EventBusSettings:HostAddress"]}");
Console.WriteLine($"------------------- {builder.Configuration["GrpcSettings:DiscountUrl"]}");

builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();


app.UseCustomExceptionHandling();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
