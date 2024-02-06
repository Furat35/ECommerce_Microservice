using Ordering.API.Extensions;
using Ordering.Application.Extensions;
using Ordering.Infrastructure.Extensions;
using Ordering.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MigrateDatabase<OrderContext>((context, services) =>
    {
        var logger = services.GetService<ILogger<OrderContextSeed>>();
        OrderContextSeed
        .SeedAsync(context, logger)
        .Wait();
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
