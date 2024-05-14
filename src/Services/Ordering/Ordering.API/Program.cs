using Ordering.API.Extensions;
using Ordering.Application.Extensions;
using Ordering.Infrastructure.Extensions;
using Ordering.Infrastructure.Persistence;
using Shared.Middleware;

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine($"------------------- {builder.Environment.EnvironmentName}");
Console.WriteLine($"------------------- {builder.Configuration["ConnectionStrings:OrderingConnectionString"]}");
builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.MigrateDatabase<OrderContext>((context, services) =>
{
    var logger = services.GetService<ILogger<OrderContextSeed>>();
    OrderContextSeed
    .SeedAsync(context, logger)
    .Wait();
});

app.UseCustomExceptionHandling();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
