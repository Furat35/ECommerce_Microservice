using ECommerce.Services;
using ECommerce.UI.Services;
using ECommerce.UI.Services.Contracts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Shared.Middleware;
using ECommerce.UI.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

builder.Services.AddApiServices(builder.Configuration);

Console.WriteLine(builder.Configuration["Deger"] + "------");
Console.WriteLine(builder.Environment.EnvironmentName + "------");


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

//app.UseCustomExceptionHandling();
app.UseExceptionHandler("/Error");

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.MapDefaultControllerRoute();

app.Run();
