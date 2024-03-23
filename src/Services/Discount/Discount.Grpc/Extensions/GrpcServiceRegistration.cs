using Discount.Grpc.Repositories;

namespace Discount.Grpc.Extensions
{
    public static class GrpcServiceRegistration
    {
        public static void AddGrpcServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            services.AddGrpc();
            services.AddAutoMapper(typeof(Program));
        }
    }
}
