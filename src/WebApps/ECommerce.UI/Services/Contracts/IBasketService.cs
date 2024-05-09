using ECommerce.UI.Models.Dtos.ViewModels.Baskets;

namespace ECommerce.UI.Services.Contracts
{
    public interface IBasketService
    {

        Task<ShoppingCartModel> GetBasket();
        Task<ShoppingCartModel> UpdateBasket(ShoppingCartModel model);
        Task<ShoppingCartModel> RefreshBasket();
        Task<ShoppingCartModel> RemoveItemFromBasket(string productId);
        Task<ShoppingCartModel> DecreaseItemQuantity(string productId);
        Task CheckoutBasket(BasketCheckoutModel model);
    }
}
