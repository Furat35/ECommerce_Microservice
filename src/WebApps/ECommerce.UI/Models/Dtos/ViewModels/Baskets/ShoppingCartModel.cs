namespace ECommerce.UI.Models.Dtos.ViewModels.Baskets
{
    public class ShoppingCartModel
    {
        public ShoppingCartModel()
        {

        }

        public List<ShoppingCartItemModel> Items { get; set; } = new List<ShoppingCartItemModel>();

        public decimal TotalPrice
        {
            get
            {
                decimal totalPrice = 0;
                foreach (var item in Items)
                {
                    totalPrice += item.Price * item.Quantity;
                }
                return totalPrice;
            }
        }
    }
}
