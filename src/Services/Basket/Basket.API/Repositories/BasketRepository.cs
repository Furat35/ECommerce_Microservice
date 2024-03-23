using Basket.API.Entities;
using Basket.API.ExternalApiCalls.Contracts;
using Basket.API.GrpcServices;
using EventBus.Message.Events;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Exceptions;
using Shared.Helpers;
using Shared.Models.Addresses;
using Shared.Models.Basket;
using Shared.Models.PaymentCards;
using System.Text.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DiscountGrpcService _discountGrpcService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ICatalogExternalService _catalogExternalService;

        public BasketRepository(IDistributedCache redisCache, IHttpContextAccessor httpContextAccessor, DiscountGrpcService discountGrpcService,
            IPublishEndpoint publishEndpoint, ICatalogExternalService catalogExternalService)
        {
            _redisCache = redisCache;
            _httpContextAccessor = httpContextAccessor;
            _discountGrpcService = discountGrpcService;
            _publishEndpoint = publishEndpoint;
            _catalogExternalService = catalogExternalService;
        }

        public async Task DeleteBasket(string userId)
        {
            await _redisCache.RemoveAsync(userId.ToUpperInvariant());
        }

        public async Task<ShoppingCart> GetBasket(string userId)
        {
            var basket = await _redisCache.GetStringAsync(userId.ToUpperInvariant());
            if (string.IsNullOrEmpty(basket))
                return null;

            return JsonSerializer.Deserialize<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            var userId = _httpContextAccessor.HttpContext.User.GetActiveUserId();
            var activeBasket = await GetBasket(userId) ?? new ShoppingCart();
            foreach (var item in basket.Items)
            {
                var productIsInBasket = activeBasket.Items.FirstOrDefault(_ => _.ProductId == item.ProductId);
                if (productIsInBasket != null)
                {
                    productIsInBasket.Quantity += item.Quantity;
                    continue;
                }
                var product = await _catalogExternalService.GetProductById(item.ProductId);
                if (product is null)
                    continue;
                var coupon = await _discountGrpcService.GetDiscount(item.ProductId);
                item.Price = product.Price - coupon.Amount;
                activeBasket.Items.Add(item);
            }
            await _redisCache.SetStringAsync(userId.ToUpperInvariant(), JsonSerializer.Serialize(activeBasket));

            return activeBasket;
        }

        public async Task<ShoppingCart> RemoveItemFromBasket(string productId)
        {
            var userId = _httpContextAccessor.HttpContext.User.GetActiveUserId();
            var activeBasket = await GetBasket(userId);
            var productToRemove = activeBasket.Items.FirstOrDefault(_ => _.ProductId == productId);
            if (productToRemove is null)
                throw new BadRequestException();

            activeBasket.Items.Remove(productToRemove);
            await _redisCache.SetStringAsync(userId.ToUpperInvariant(), JsonSerializer.Serialize(activeBasket));

            return activeBasket;
        }

        public async Task CheckoutBasket(BasketCheckout basketCheckout)
        {
            var userId = _httpContextAccessor.HttpContext.User.GetActiveUserId();
            var basket = await RefreshBasket(userId);
            if (basket is null)
                throw new BadRequestException();

            var eventMessage = new BasketCheckoutEvent
            {
                TotalPrice = basket.TotalPrice,
                UserId = Guid.Parse(userId),
                Mail = userId,
                Name = basketCheckout.Name,
                Surname = basketCheckout.Surname,
                Address = new AddressCheckoutDto
                {
                    AddressLine = basketCheckout.Address.AddressLine,
                    Country = basketCheckout.Address.Country,
                    State = basketCheckout.Address.State,
                    ZipCode = basketCheckout.Address.ZipCode
                },
                PaymentCard = new PaymentCardCheckoutDto
                {
                    CardName = basketCheckout.PaymentCard.CardName,
                    CardNumber = basketCheckout.PaymentCard.CardNumber,
                    Expiration = basketCheckout.PaymentCard.Expiration,
                    CVV = basketCheckout.PaymentCard.CVV,
                    PaymentMethod = basketCheckout.PaymentCard.PaymentMethod,
                },
                OrderItems = new List<ShoppingCartItemCheckoutDto>()
            };
            if (basket.Items.Count == 0)
                throw new BadRequestException("Update your cart!");
            basket.Items.ForEach(item =>
            {
                eventMessage.OrderItems.Add(new ShoppingCartItemCheckoutDto
                {
                    Price = item.Price,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                });
            });

            eventMessage.TotalPrice = basket.TotalPrice;
            await _publishEndpoint.Publish(eventMessage);
            await DeleteBasket(userId);
        }

        public async Task<ShoppingCart> RefreshBasket(string userId)
        {
            var previousBasket = await GetBasket(userId);
            var activeBasket = new ShoppingCart();
            foreach (var item in previousBasket.Items)
            {
                var product = await _catalogExternalService.GetProductById(item.ProductId);
                if (product != null)
                {
                    var coupon = await _discountGrpcService.GetDiscount(item.ProductId);
                    item.Price = product.Price - coupon.Amount;
                    activeBasket.Items.Add(item);
                }
            }
            await _redisCache.SetStringAsync(userId.ToUpperInvariant(), JsonSerializer.Serialize(activeBasket));

            return activeBasket;
        }

    }
}
