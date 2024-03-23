using Ordering.Domain.Common;

namespace Ordering.Domain.Entities
{
    public class Order : EntityBase
    {
        public decimal TotalPrice { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
        public Address Address { get; set; }
        public PaymentCard PaymentCard { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
