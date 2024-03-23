using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Authentication.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder, int retryCount) where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

                    //Will be removed 
                    context.Database.Migrate();
                    seeder(context, services);

                    logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
                }
                catch (SqlException ex)
                {
                    if (retryCount == 0)
                        return host;
                    logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
                    Thread.Sleep(800);
                    MigrateDatabase(host, seeder, --retryCount);
                }
            }

            return host;
        }
    }
}
