using Basket.API.ExternalApiCalls;
using Basket.API.ExternalApiCalls.Contracts;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace Basket.API.Extensions
{
    public static class ApiServiceRegistration
    {
        public static void AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers(opt =>
            {
                opt.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = false;
            });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<ICatalogExternalService, CatalogExternalService>();
            services.AddScoped<DiscountGrpcService>();
            services.AddHttpContextAccessor();
            services.AddHttpClient("Catalog.Api", client =>
            {
                client.BaseAddress = new Uri(configuration["Catalog.Api"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            services.AddHttpClient<IPaymentExternalService, PaymentExternalService>(c =>
                        c.BaseAddress = new Uri(configuration["Payment.Api"]));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetValue<string>("CacheSettings:ConnectionString");
            });
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(o => o.Address = new Uri(configuration["GrpcSettings:DiscountUrl"]));

            // Masstransit configurations
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(configuration["EventBusSettings:HostAddress"]);
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
