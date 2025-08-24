using Microsoft.AspNetCore.Mvc;
using BackendApi.DTOs;
using BackendApi.Services.Interfaces;

namespace BackendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns>List of products</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            _logger.LogInformation("Getting all products");
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        /// <summary>
        /// Search products by term
        /// </summary>
        /// <param name="searchTerm">Search term to filter products by (searches across name, description, category, brand, SKU, colors, sizes, and availability status)</param>
        /// <returns>List of products matching the search term</returns>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> SearchProducts([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                _logger.LogWarning("Search attempted with empty or null search term");
                return BadRequest("Search term cannot be empty or null");
            }

            _logger.LogInformation("Searching products with term: {SearchTerm}", searchTerm);
            var products = await _productService.SearchProductsAsync(searchTerm);
            _logger.LogInformation("Found {Count} products matching search term: {SearchTerm}", 
                products.Count(), searchTerm);
            
            return Ok(products);
        }

        /// <summary>
        /// Get a specific product by ID
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Product details</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductDto>> GetProduct(Guid id)
        {
            _logger.LogInformation("Getting product with ID: {ProductId}", id);
            
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found", id);
                return NotFound($"Product with ID {id} not found");
            }

            return Ok(product);
        }

        /// <summary>
        /// Get a product by SKU
        /// </summary>
        /// <param name="sku">Product SKU</param>
        /// <returns>Product details</returns>
        [HttpGet("by-sku/{sku}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductDto>> GetProductBySKU(string sku)
        {
            _logger.LogInformation("Getting product with SKU: {ProductSKU}", sku);
            
            var product = await _productService.GetProductBySKUAsync(sku);
            if (product == null)
            {
                _logger.LogWarning("Product with SKU {ProductSKU} not found", sku);
                return NotFound($"Product with SKU {sku} not found");
            }

            return Ok(product);
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        /// <param name="createProductDto">Product data</param>
        /// <returns>Created product</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto createProductDto)
        {
            _logger.LogInformation("Creating new product with SKU: {ProductSKU}", createProductDto.SKU);
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdProduct = await _productService.CreateProductAsync(createProductDto);
            
            _logger.LogInformation("Product created successfully with ID: {ProductId}", createdProduct.Id);
            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
        }

        /// <summary>
        /// Update an existing product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="updateProductDto">Updated product data</param>
        /// <returns>No content on success</returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductDto updateProductDto)
        {
            _logger.LogInformation("Updating product with ID: {ProductId}", id);
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedProduct = await _productService.UpdateProductAsync(id, updateProductDto);
            if (updatedProduct == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found for update", id);
                return NotFound($"Product with ID {id} not found");
            }

            _logger.LogInformation("Product with ID {ProductId} updated successfully", id);
            return NoContent();
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>No content on success</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            _logger.LogInformation("Deleting product with ID: {ProductId}", id);
            
            var deleted = await _productService.DeleteProductAsync(id);
            if (!deleted)
            {
                _logger.LogWarning("Product with ID {ProductId} not found for deletion", id);
                return NotFound($"Product with ID {id} not found");
            }

            _logger.LogInformation("Product with ID {ProductId} deleted successfully", id);
            return NoContent();
        }

        /// <summary>
        /// Generate bulk products
        /// </summary>
        /// <param name="count">Number of products to generate (1-10000), defaults to 1000</param>
        /// <param name="template">Product template to use as base for generation (optional)</param>
        /// <returns>Number of products created</returns>
        [HttpPost("generate/{count:int?}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<object>> BulkGenerateProducts(int count = 1000, [FromBody] ProductDto? template = null)
        {
            _logger.LogInformation("Starting bulk generation of {Count} products", count);
            
            if (count < 1 || count > 10000)
            {
                return BadRequest("Count must be between 1,000 and 10,000");
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var startTime = DateTime.UtcNow;
            var productsCreated = await _productService.BulkGenerateProductsAsync(count, template);
            var endTime = DateTime.UtcNow;
            var duration = endTime - startTime;

            _logger.LogInformation("Successfully generated {ProductsCreated} products in {Duration}ms", 
                productsCreated, duration.TotalMilliseconds);

            return Ok(new
            {
                Message = "Products generated successfully",
                ProductsCreated = productsCreated,
                RequestedCount = count,
                GenerationTimeMs = duration.TotalMilliseconds,
                Timestamp = endTime,
                UsedTemplate = template != null
            });
        }
    }
}
