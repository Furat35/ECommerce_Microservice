using Ordering.Domain.Common;

namespace Ordering.Domain.Entities
{
    public class OrderItem : EntityBase
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
    }
}
