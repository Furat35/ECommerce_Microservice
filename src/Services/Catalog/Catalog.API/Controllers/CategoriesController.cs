using Catalog.API.Entities;
using Catalog.API.Helpers.Filters.Categories;
using Catalog.API.Models.Categories;
using Catalog.API.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryRepository categoryRepository) : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Category>))]
        public async Task<IActionResult> GetCategories([FromQuery] CategoryRequestFilter filters)
        {
            var Categorys = await _categoryRepository.GetCategoriesAsync(filters);
            return Ok(Categorys);
        }

        [HttpGet("{id:length(24)}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryById(string id)
        {
            var Category = await _categoryRepository.GetCategoryAsync(id);
            return Ok(Category);
        }

        [HttpPost]
        [Authorize(Roles = $"{Role.Admin}")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(IEnumerable<Category>))]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryAddDto Category)
        {
            await _categoryRepository.CreateCategoryAsync(Category);
            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
        [Authorize(Roles = $"{Role.Admin}")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryUpdateDto Category)
        {
            return Ok(await _categoryRepository.UpdateCategoryAsync(Category));
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
        [Authorize(Roles = $"{Role.Admin}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            return Ok(await _categoryRepository.DeleteCategoryAsync(id));
        }
    }
}
