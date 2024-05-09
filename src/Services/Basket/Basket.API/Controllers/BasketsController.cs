using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.Helpers;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;

        public BasketsController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }


        [HttpGet(Name = "GetBasket")]
        [Authorize(Roles = $"{Role.User}")]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBasket()
        {
            var basket = await _basketRepository.GetBasket(User.GetActiveUserId());
            return Ok(basket ?? new ShoppingCart());
        }

        [HttpPut]
        [Authorize(Roles = $"{Role.User}")]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateBasket([FromBody] ShoppingCart basket)
        {
            var activeBasket = await _basketRepository.UpdateBasket(basket);
            return Ok(activeBasket);
        }

        [HttpPut("refresh")]
        [Authorize(Roles = $"{Role.User}")]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshBasket()
        {
            var basket = await _basketRepository.RefreshBasket(HttpContext.User.GetActiveUserId());
            return Ok(basket);
        }

        [HttpDelete("remove/{productId}")]
        [Authorize(Roles = $"{Role.User}")]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveItemFromBasket(string productId)
        {
            var activeBasket = await _basketRepository.RemoveItemFromBasket(productId);
            return Ok(activeBasket);
        }

        [HttpDelete(Name = "DeleteBasket")]
        [Authorize(Roles = $"{Role.User}")]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveBasket()
        {
            await _basketRepository.DeleteBasket(User.GetActiveUserId());
            return Ok();
        }

        [HttpPost("[action]/{productId}")]
        [Authorize(Roles = $"{Role.User}")]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
        public async Task<IActionResult> DecreaseItemQuantity(string productId)
        {
            var activeBasket = await _basketRepository.DecreaseItemQuantityByOne(productId, HttpContext.User.GetActiveUserId());
            return Ok(activeBasket);
        }

        [HttpPost("[action]", Name = "CheckoutBasket")]
        [Authorize(Roles = $"{Role.User}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CheckoutBasket([FromBody] BasketCheckout basketCheckout)
        {
            await _basketRepository.CheckoutBasket(basketCheckout);
            return Accepted();
        }
    }
}
