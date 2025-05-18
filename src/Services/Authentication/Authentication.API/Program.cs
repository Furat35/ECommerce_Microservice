using Authentication.API.DataAccess.Contexts;
using Authentication.API.Extensions;
using Shared.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

app.MigrateDatabase<AuthenticationContext>((context, services) =>
{
    var logger = services.GetService<ILogger<AuthenticationContextSeed>>();
    AuthenticationContextSeed
    .SeedAsync(context)
    .Wait();
}, retryCount: 7);

app.UseCustomExceptionHandling();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
