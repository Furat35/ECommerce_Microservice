using Discount.Grpc.Extensions;
using Discount.Grpc.Services;
using Shared.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpcServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MigrateDatabase<Program>();
}

app.UseCustomExceptionHandling();
app.MapGrpcService<DiscountService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
