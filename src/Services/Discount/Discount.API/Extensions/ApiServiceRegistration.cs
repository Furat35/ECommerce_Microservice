using Discount.API.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace Discount.API.Extensions
{
    public static class ApiServiceRegistration
    {
        public static void AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddScoped<IDiscountRepository, DiscountRepository>();

            // Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["JWTAuth:ValidIssuerURL"],
                        ValidAudience = configuration["JWTAuth:ValidAudienceURL"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTAuth:SecretKey"])),
                    };
                });

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
