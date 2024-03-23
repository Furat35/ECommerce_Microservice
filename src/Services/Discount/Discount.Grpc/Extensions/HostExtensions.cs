using Npgsql;

namespace Discount.Grpc.Extensions
{
    public static class HostExtensions
    {

        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var configuration = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILogger<TContext>>();
            try
            {
                logger.LogInformation("Migrating postgresql database.");
                using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                connection.Open();
                using var command = new NpgsqlCommand
                {
                    Connection = connection
                };
                command.CommandText = "DROP TABLE IF EXISTS Coupon";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE Coupon (Id SERIAL PRIMARY KEY ,"
                                                                + "ProductId VARCHAR(24) NOT NULL,"
                                                                + "Description TEXT,"
                                                                + "Amount INT)";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO Coupon(ProductId, Description, Amount) VALUES ('124','IPhone Discount', 150)";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO Coupon(ProductId, Description, Amount) VALUES ('512','Samsung Discount', 100)";
                command.ExecuteNonQuery();

                logger.LogInformation("Migrated postgresql database.");
            }
            catch (NpgsqlException ex)
            {
                logger.LogError(ex, "An error occured while migrating the postgresql database.");
                if (retryForAvailability < 50)
                {
                    retryForAvailability++;
                    Thread.Sleep(2000);
                    MigrateDatabase<TContext>(host, retryForAvailability);
                }

            }

            return host;
        }
    }
}
