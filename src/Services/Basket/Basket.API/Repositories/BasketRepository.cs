using Basket.API.Entities;
using Basket.API.ExternalApiCalls.Contracts;
using Basket.API.GrpcServices;
using EventBus.Message.Events;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Exceptions;
using Shared.Extensions;
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
        private readonly IPaymentExternalService _paymentExternalService;
        private readonly IValidator<ShoppingCart> _shoppingCartValidator;
        private readonly IValidator<ShoppingCartItem> _shoppingCartItemValidator;
        private readonly IValidator<BasketCheckout> _basketCheckoutValidator;

        public BasketRepository(IDistributedCache redisCache, IHttpContextAccessor httpContextAccessor, DiscountGrpcService discountGrpcService,
            IPublishEndpoint publishEndpoint, ICatalogExternalService catalogExternalService, IPaymentExternalService paymentExternalService, IValidator<ShoppingCart> shoppingCartValidator,
            IValidator<ShoppingCartItem> shoppingCartItemValidator, IValidator<BasketCheckout> basketCheckoutValidator)
        {
            _redisCache = redisCache;
            _httpContextAccessor = httpContextAccessor;
            _discountGrpcService = discountGrpcService;
            _publishEndpoint = publishEndpoint;
            _catalogExternalService = catalogExternalService;
            _paymentExternalService = paymentExternalService;
            _shoppingCartValidator = shoppingCartValidator;
            _shoppingCartItemValidator = shoppingCartItemValidator;
            _basketCheckoutValidator = basketCheckoutValidator;
        }

        public async Task DeleteBasket(string userId)
            => await _redisCache.RemoveAsync(userId.ToUpperInvariant());

        public async Task<ShoppingCart> GetBasket(string userId)
        {
            var basket = await _redisCache.GetStringAsync(userId.ToUpperInvariant());
            return !string.IsNullOrEmpty(basket) ? JsonSerializer.Deserialize<ShoppingCart>(basket) : new ShoppingCart();
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await CustomFluentValidationErrorHandling.ValidateAndThrowAsync(basket, _shoppingCartValidator);
            await ThrowBadRequestIfShoppingCartItemNotValid(basket.Items);

            var userId = _httpContextAccessor.HttpContext.User.GetActiveUserId();
            var activeBasket = await UpdateShoppingCartItems(basket);
            await _redisCache.SetStringAsync(userId.ToUpperInvariant(), JsonSerializer.Serialize(activeBasket));

            return activeBasket;
        }

        public async Task<ShoppingCart> RemoveItemFromBasket(string productId)
        {
            var userId = _httpContextAccessor.HttpContext.User.GetActiveUserId();
            var activeBasket = await GetBasket(userId);
            var productToRemove = activeBasket.Items.FirstOrDefault(_ => _.ProductId == productId);
            if (productToRemove is null)
                throw new BadRequestException("Ürün sepette bulunmuyor!");

            activeBasket.Items.Remove(productToRemove);
            await _redisCache.SetStringAsync(userId.ToUpperInvariant(), JsonSerializer.Serialize(activeBasket));

            return activeBasket;
        }

        public async Task<ShoppingCart> DecreaseItemQuantityByOne(string productId, string userId)
        {
            var basket = await GetBasket(userId);
            var basketItem = basket.Items.FirstOrDefault(_ => _.ProductId == productId);
            if (basketItem is null)
                throw new BadRequestException("Ürün sepette bulunmuyor! Geçerli ürün giriniz!");

            if (basketItem.Quantity <= 1)
                return await RemoveItemFromBasket(productId);

            basketItem.Quantity--;
            await _redisCache.SetStringAsync(userId.ToUpperInvariant(), JsonSerializer.Serialize(basket));

            return basket;
        }

        public async Task CheckoutBasket(BasketCheckout basketCheckout)
        {
            await CustomFluentValidationErrorHandling.ValidateAndThrowAsync(basketCheckout, _basketCheckoutValidator);
            var userId = _httpContextAccessor.HttpContext.User.GetActiveUserId();
            var basket = await RefreshBasket(userId);
            var paymentIsSuccess = await _paymentExternalService.ProcessPayment();
            if (paymentIsSuccess)
            {

                var eventMessage = new BasketCheckoutEvent
                {
                    TotalPrice = basket.TotalPrice,
                    UserId = Guid.Parse(userId),
                    Mail = _httpContextAccessor.HttpContext.User.GetActiveUserMail(),
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
        }

        public async Task<ShoppingCart> RefreshBasket(string userId)
        {
            var previousBasket = await GetBasket(userId);
            var activeBasket = new ShoppingCart();
            if (previousBasket.Items.Count == 0)
                throw new BadRequestException("Sepette ürün bulunmuyor");

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

        private async Task<ShoppingCart> UpdateShoppingCartItems(ShoppingCart basket)
        {
            var userId = _httpContextAccessor.HttpContext.User.GetActiveUserId();
            var activeBasket = await GetBasket(userId) ?? new ShoppingCart();

            foreach (var item in basket.Items)
            {
                var productIsInBasket = activeBasket.Items.FirstOrDefault(_ => _.ProductId == item.ProductId);
                if (productIsInBasket != null)
                {
                    productIsInBasket.Quantity += item.Quantity;
                }
                else
                {
                    var product = await _catalogExternalService.GetProductById(item.ProductId);
                    if (product is null)
                        continue;
                    var coupon = await _discountGrpcService.GetDiscount(item.ProductId);
                    item.Price = product.Price - coupon.Amount;
                    activeBasket.Items.Add(item);
                }
            }

            return activeBasket;
        }

        private async Task ThrowBadRequestIfShoppingCartItemNotValid(List<ShoppingCartItem> shoppingCartItems)
        {
            foreach (var item in shoppingCartItems)
                await CustomFluentValidationErrorHandling.ValidateAndThrowAsync(item, _shoppingCartItemValidator);
        }
    }
}
