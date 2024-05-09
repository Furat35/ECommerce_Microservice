namespace ECommerce.UI.Models.Dtos.ViewModels.Baskets
{
    public class ShoppingCartItemModel
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
    }
}
