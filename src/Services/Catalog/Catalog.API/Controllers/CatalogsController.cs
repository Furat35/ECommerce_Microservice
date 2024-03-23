using Catalog.API.Entities;
using Catalog.API.Helpers.Filters;
using Catalog.API.Models.Product;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public CatalogsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
        public async Task<IActionResult> GetProducts([FromQuery] ProductRequestFilter filters)
        {
            var products = await _productRepository.GetProductsAsync(filters);
            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductById(string id)
        {
            var product = await _productRepository.GetProductAsync(id);
            return Ok(product);
        }

        [HttpGet("[action]/{category}", Name = "GetProductsByCategory")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
        public async Task<IActionResult> GetProductsByCategory(string category, [FromQuery] ProductRequestFilter filters)
        {
            var products = await _productRepository.GetProductsByCategoryAsync(category, filters);
            return Ok(products);
        }

        [HttpPost]
        [Authorize(Roles = $"{Role.Admin}")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(IEnumerable<Product>))]
        public async Task<IActionResult> CreateProduct([FromBody] ProductAddDto product)
        {
            await _productRepository.CreateProductAsync(product);
            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [Authorize(Roles = $"{Role.Admin}")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateDto product)
        {
            return Ok(await _productRepository.UpdateProductAsync(product));
        }


        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [Authorize(Roles = $"{Role.Admin}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            return Ok(await _productRepository.DeleteProductAsync(id));
        }
    }
}
