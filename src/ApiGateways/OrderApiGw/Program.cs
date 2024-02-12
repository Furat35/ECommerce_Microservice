using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);
//builder.Services.AddLogging(configure =>
//{
//    //configure.addcon(builder.Configuration);
//    configure.AddConfiguration(builder.Configuration.GetSection("Logging"));
//    configure.AddConsole();
//    configure.AddDebug();
//});

builder.Services
    .AddOcelot()
    .AddCacheManager(x => x.WithDictionaryHandle()); ;

var app = builder.Build();


app.MapGet("/", () => "Hello World!");

await app.UseOcelot();

app.Run();
