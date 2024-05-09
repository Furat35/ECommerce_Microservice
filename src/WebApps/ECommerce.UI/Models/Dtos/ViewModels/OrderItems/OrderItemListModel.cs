namespace ECommerce.UI.Models.Dtos.ViewModels.OrderItems
{
    public class OrderItemListModel
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
    }
}
