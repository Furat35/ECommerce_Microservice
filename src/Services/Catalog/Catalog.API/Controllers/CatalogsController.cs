using Catalog.API.Entities;
using Catalog.API.Helpers.Filters.Products;
using Catalog.API.Models.Products;
using Catalog.API.Repositories.Contracts;
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

        [HttpGet("category", Name = "GetProductsByCategoryId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductsByCategoryId([FromQuery] ProductRequestFilter filters)
        {
            var product = await _productRepository.GetProductsByCategoryAsync(filters);
            return Ok(product);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductById(string id)
        {
            var product = await _productRepository.GetProductAsync(id);
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = $"{Role.Admin}")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(IEnumerable<Product>))]
        public async Task<IActionResult> CreateProduct([FromForm] ProductAddDto product)
        {
            var productId = await _productRepository.CreateProductAsync(product);
            return Ok(productId);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [Authorize(Roles = $"{Role.Admin}")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateDto product)
        {
            return Ok(await _productRepository.UpdateProductAsync(product));
        }

        [HttpPut("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        public async Task<IActionResult> UpdateProductPhoto(string productId)
        {
            var isUploaded = await _productRepository.UpdateProductPhoto(productId);
            return Ok(isUploaded);
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
