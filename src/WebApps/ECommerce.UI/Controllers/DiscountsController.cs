using ECommerce.UI.Models.Dtos.ViewModels.Discounts;
using ECommerce.UI.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.UI.Controllers
{
    public class DiscountsController : Controller
    {
        private readonly IDiscountService _discountRepository;

        public DiscountsController(IDiscountService discountRepository)
        {
            _discountRepository = discountRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetDiscount([FromQuery] string productId)
        {
            var discount = await _discountRepository.GetDiscountByProductId(productId);
            return Ok(discount);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiscount([FromForm] DiscountCreateModel discount)
        {
            await _discountRepository.CreateDiscount(discount);
            return RedirectToAction(nameof(ProductsController.ProductTransactions), "Products");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDiscount([FromForm] DiscountUpdateModel discount)
        {
            await _discountRepository.UpdateDiscount(discount);
            return RedirectToAction(nameof(ProductsController.ProductTransactions), "Products");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDiscount(string discountId)
        {
            await _discountRepository.DeleteDiscount(discountId);
            return RedirectToAction(nameof(ProductsController.ProductTransactions), "Products");
        }
    }
}
