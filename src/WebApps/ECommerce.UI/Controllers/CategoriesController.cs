using ECommerce.UI.Helpers.Filters;
using ECommerce.UI.Models.Dtos.ViewModels.Categories;
using ECommerce.UI.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;

namespace ECommerce.UI.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> CategoryTransactions([FromQuery] CategoryRequestFilter filters)
        {
            var categories = await _categoryService.GetCategories(filters);
            return View(categories);
        }

        public async Task<IActionResult> CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = $"{Role.Admin}")]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryCreateModel product)
        {
            await _categoryService.CreateCategory(product);
            return RedirectToAction(nameof(CategoryTransactions));
        }


        [Authorize(Roles = $"{Role.Admin}")]
        [HttpPost]
        public async Task<IActionResult> UpdateCategory([FromForm] CategoryUpdateModel product)
        {
            bool updateResult = await _categoryService.UpdateCategory(product);
            return RedirectToAction(nameof(CategoryTransactions));
        }

        [Authorize(Roles = $"{Role.Admin}")]
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            bool deleteResult = await _categoryService.DeleteCategory(id);
            return RedirectToAction(nameof(CategoryTransactions));
        }
    }
}
