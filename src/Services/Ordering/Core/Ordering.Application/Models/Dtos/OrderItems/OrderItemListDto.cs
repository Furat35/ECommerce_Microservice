namespace Ordering.Application.Models.Dtos.OrderItems
{
    public class OrderItemListDto
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
    }
}
