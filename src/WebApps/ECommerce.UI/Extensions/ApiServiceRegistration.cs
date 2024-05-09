using ECommerce.Services;
using ECommerce.UI.Services;
using ECommerce.UI.Services.Contracts;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ECommerce.UI.Extensions
{
    public static class ApiServiceRegistration
    {
        public static void AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();
            services.AddHttpContextAccessor();
            services.AddScoped<IAuthService, AuthService>();
            services.AddHttpClient<IAuthService, AuthService>(c =>
                        c.BaseAddress = new Uri(configuration["ApiSettings:GatewayAddress"]));

            services.AddHttpClient<ICatalogService, CatalogService>(c =>
                        c.BaseAddress = new Uri(configuration["ApiSettings:GatewayAddress"]));

            services.AddHttpClient<IBasketService, BasketService>(c =>
                        c.BaseAddress = new Uri(configuration["ApiSettings:GatewayAddress"]));

            services.AddHttpClient<IUserService, UserService>(c =>
                        c.BaseAddress = new Uri(configuration["ApiSettings:GatewayAddress"]));


            services.AddHttpClient<IOrderService, OrderService>(c =>
                        c.BaseAddress = new Uri(configuration["ApiSettings:GatewayAddress"]));

            services.AddHttpClient<ICategoryService, CategoryService>(c =>
                        c.BaseAddress = new Uri(configuration["ApiSettings:GatewayAddress"]));

            services.AddHttpClient<IDiscountService, DiscountService>(c =>
                        c.BaseAddress = new Uri(configuration["ApiSettings:GatewayAddress"]));

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
               .AddCookie(options =>
               {
                   options.LoginPath = "/Auth/Login"; // Specify your custom login page URL here
               });
        }
    }
}
