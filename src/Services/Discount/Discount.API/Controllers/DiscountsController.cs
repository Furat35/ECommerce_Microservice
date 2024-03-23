using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;

namespace Discount.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountsController(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        [HttpGet("{discountId}", Name = "GetDiscount")]
        [ProducesResponseType(typeof(Coupon), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Coupon), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDiscount(string discountId)
        {
            var coupon = await _discountRepository.GetDiscount(discountId);
            return Ok(coupon);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Coupon), StatusCodes.Status201Created)]
        [Authorize(Roles = $"{Role.Admin}")]
        public async Task<IActionResult> CreateDiscount([FromBody] Coupon coupon)
        {
            await _discountRepository.CreateDiscount(coupon);
            return CreatedAtRoute("GetDiscount", new { DiscountId = coupon.Id }, coupon);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Coupon), StatusCodes.Status200OK)]
        [Authorize(Roles = $"{Role.Admin}")]
        public async Task<IActionResult> UpdateDiscount([FromBody] Coupon coupon)
        {
            return Ok(await _discountRepository.UpdateDiscount(coupon));
        }

        [HttpDelete("{discountId}", Name = "DeleteDiscount")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [Authorize(Roles = $"{Role.Admin}")]
        public async Task<IActionResult> DeleteDiscount(string discountId)
        {
            return Ok(await _discountRepository.DeleteDiscount(discountId));
        }
    }
}
