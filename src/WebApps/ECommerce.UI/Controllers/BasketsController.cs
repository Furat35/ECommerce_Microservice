using ECommerce.UI.Models.Dtos.ViewModels.Baskets;
using ECommerce.UI.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using System.Security.Claims;

namespace ECommerce.UI.Controllers
{
    [Authorize]
    public class BasketsController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IUserService _userService;
        public BasketsController(IBasketService basketService, IUserService userService)
        {
            _basketService = basketService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var basket = await _basketService.GetBasket();
            return View(basket);
        }

        [HttpPost]
        [Authorize(Roles = $"{Role.User}")]
        public async Task<IActionResult> AddToCart([FromForm] ShoppingCartItemModel shoppingCartItem)
        {
            var shoppingCart = new ShoppingCartModel();
            shoppingCart.Items.Add(shoppingCartItem);
            var updatedbasket = await _basketService.UpdateBasket(shoppingCart);

            return View(nameof(Index), updatedbasket);
        }

        [HttpPost]
        [Authorize(Roles = $"{Role.User}")]
        public async Task<IActionResult> RemoveItemFromBasket(string productId)
        {
            var basket = await _basketService.RemoveItemFromBasket(productId);
            return View(nameof(Index), basket);
        }

        [HttpPost]
        [Authorize(Roles = $"{Role.User}")]
        public async Task<IActionResult> DecreaseItemQuantity(string productId)
        {
            var basket = await _basketService.DecreaseItemQuantity(productId);
            return View(nameof(Index), basket);
        }

        [Authorize(Roles = $"{Role.User}")]
        public async Task<IActionResult> CheckoutBasket()
        {
            var user = await _userService.GetUserById(HttpContext.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier).Value);
            var basket = await _basketService.RefreshBasket();
            var viewModel = (new BasketCheckoutModel(), basket, user);
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = $"{Role.User}")]
        public async Task<IActionResult> CheckoutBasket(BasketCheckoutModel basketCheckout)
        {
            await _basketService.CheckoutBasket(basketCheckout);
            return View("Confirmation");
        }
    }
}
