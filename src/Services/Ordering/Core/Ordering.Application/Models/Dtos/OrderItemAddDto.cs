namespace Ordering.Application.Models.Dtos
{
    public class OrderItemAddDto
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
    }
}
