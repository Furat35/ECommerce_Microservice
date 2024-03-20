using Authentication.API.Exceptions;
using Authentication.API.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


#region Hata yakalama
app.UseExceptionHandler(
    options =>
    {
        options.Run(async context =>
        {
            context.Response.ContentType = "application/json";
            var exceptionObject = context.Features.Get<IExceptionHandlerFeature>();

            if (exceptionObject != null)
            {
                context.Response.StatusCode = exceptionObject.Error switch
                {
                    BadRequestException ex => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                };
                var errorMessage = $"{exceptionObject.Error.Message}";
                if (context.Response.StatusCode >= 500)
                    errorMessage = "Sunucu tarafýnda beklenmeyen bir hata oluþtu! Tekrar deneyiniz.";

                await context.Response
                    .WriteAsync(JsonSerializer.Serialize(new
                    {
                        context.Response.StatusCode,
                        ErrorMessage = errorMessage
                    }))
                    .ConfigureAwait(false);
            }
        });
    }
);
#endregion

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
