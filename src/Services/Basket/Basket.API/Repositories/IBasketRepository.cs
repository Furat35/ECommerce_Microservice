using Basket.API.Entities;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasket(string userId);
        Task<ShoppingCart> UpdateBasket(ShoppingCart basket);
        Task<ShoppingCart> RemoveItemFromBasket(string productId);
        Task CheckoutBasket(BasketCheckout basketCheckout);
        Task DeleteBasket(string userId);
        Task<ShoppingCart> RefreshBasket(string userId);
    }
}
