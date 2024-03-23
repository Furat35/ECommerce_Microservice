using Authentication.API.DataAccess.Contexts;
using Authentication.API.Helpers;
using Authentication.API.Helpers.Common;
using Authentication.API.Services;
using Authentication.API.Services.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace Authentication.API.Extensions
{
    public static class ApiServiceRegistration
    {
        public static void AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            // DI Services
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordGenerationService, PasswordGenerationService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, JwtService>();
            services.AddHttpContextAccessor();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddDbContext<AuthenticationContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("AuthenticationConnectionString"), opt =>
                {
                    opt.EnableRetryOnFailure();
                });
            });

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


        }
    }
}
