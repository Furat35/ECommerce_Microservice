using Discount.Grpc.Extensions;
using Discount.Grpc.Services;
using Shared.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpcServices(builder.Configuration);

var app = builder.Build();

app.MigrateDatabase<Program>();

app.UseCustomExceptionHandling();
app.MapGrpcService<DiscountService>();

app.Run();
