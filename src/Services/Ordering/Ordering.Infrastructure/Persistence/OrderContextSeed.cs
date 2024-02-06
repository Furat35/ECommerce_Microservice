using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreconfiguredOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>()
            {
                new Order(){UserName = "fior", TotalPrice = 123, FirstName = "firat", LastName = "ortac", EmailAddress = "firat@gmail.com", AddressLine = "karsiyaka", Country = "türkiye", State="izmir", ZipCode= "35", CardName ="home", CardNumber="444449999933311111333", Expiration = DateTime.Now.ToString(), CVV = "123", PaymentMethod = 0, LastModifiedBy = "fior"}
            };
        }
    }
}
