using Catalog.API.Data;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace Catalog.API.Extensions
{
    public static class ApiServiceRegistration
    {
        public static void AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers(configure =>
            {
                configure.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddScoped<ICatalogContext, CatalogContext>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddHttpContextAccessor();
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
