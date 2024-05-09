using ECommerce.UI.Helpers.Filters;
using ECommerce.UI.Models.Dtos.ViewModels.Products;
using ECommerce.UI.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared.Constants;

namespace ECommerce.UI.Controllers
{
    // todo: Products should be get according to the created date, and according to istatitics of the favorite 
    public class ProductsController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly ICategoryService _categoryService;

        public ProductsController(ICatalogService catalogService, ICategoryService categoryService)
        {
            _catalogService = catalogService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> GetProducts([FromQuery] ProductRequestFilter filters)
        {
            var categories = await _categoryService.GetCategories();
            var products = await _catalogService.GetProducts(filters);
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");

            return View(products);
        }

        public async Task<IActionResult> GetProductsByCategoryId([FromQuery] ProductRequestFilter filters)
        {
            var categories = await _categoryService.GetCategories();
            var product = await _catalogService.GetProductsByCategoryId(filters);
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");

            return View(product);
        }

        public async Task<IActionResult> GetProductDetail([FromQuery] string productId)
        {
            var product = await _catalogService.GetProductById(productId);
            return View(product);
        }

        [Authorize(Roles = $"{Role.Admin}")]
        public async Task<IActionResult> CreateProduct()
        {
            var categories = await _categoryService.GetCategories();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = $"{Role.Admin}")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateModel product)
        {
            var productId = await _catalogService.CreateProduct(product);
            return RedirectToAction(nameof(GetProductDetail), new { productId });
        }


        [Authorize(Roles = $"{Role.Admin}")]
        public async Task<IActionResult> ProductTransactions([FromQuery] ProductRequestFilter filters)
        {
            var products = await _catalogService.GetProducts(filters);
            var categories = await _categoryService.GetCategories();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");

            return View(products);
        }


        [Authorize(Roles = $"{Role.Admin}")]
        [HttpPost]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductUpdateModel product)
        {
            bool updateResult = await _catalogService.UpdateProduct(product);
            return RedirectToAction(nameof(ProductTransactions));
        }

        [Authorize(Roles = $"{Role.Admin}")]
        [HttpPost]
        public async Task<IActionResult> UpdateProductPhoto(IFormFile newImageFile, string updateProductPhotoId)
        {
            bool updateResult = await _catalogService.UpdateProductPhoto(newImageFile, updateProductPhotoId);
            return RedirectToAction(nameof(ProductTransactions));
        }

        [Authorize(Roles = $"{Role.Admin}")]
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            bool deleteResult = await _catalogService.DeleteProduct(id);
            return RedirectToAction(nameof(ProductTransactions));
        }
    }
}
