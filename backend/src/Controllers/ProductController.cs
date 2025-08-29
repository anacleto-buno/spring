using Microsoft.AspNetCore.Mvc;
using BackendApi.DTOs.Product;
using BackendApi.DTOs.Common;
using BackendApi.Services.Interfaces;

namespace BackendApi.Controllers
{
    /// <summary>
    /// Product controller with comprehensive REST API operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Filter products using POST for integration test compatibility
        /// </summary>
        /// <param name="filter">Filter and pagination parameters</param>
        /// <returns>Paged list of products</returns>
        [HttpPost("filter")]
        [ProducesResponseType(typeof(PagedResult<ProductListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<ProductListDto>>> FilterProducts([FromBody] ProductFilterDto filter)
        {
            try
            {
                _logger.LogInformation("Filtering products with filter: {@Filter}", filter);
                if (filter == null)
                    filter = new ProductFilterDto();
                var result = await _productService.GetProductsAsync(filter);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid filter parameters: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get products with filtering and pagination
        /// </summary>
        /// <param name="filter">Filter and pagination parameters</param>
        /// <returns>Paged list of products</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<ProductListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<ProductListDto>>> GetProducts([FromQuery] ProductFilterDto filter)
        {
            try
            {
                _logger.LogInformation("Getting products with filter: {@Filter}", filter);
                
                if (filter == null)
                    filter = new ProductFilterDto();

                var result = await _productService.GetProductsAsync(filter);
                
                _logger.LogInformation("Retrieved {Count} products out of {Total}", 
                    result.Items.Count(), result.TotalCount);
                
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid filter parameters: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Search products by term with pagination
        /// </summary>
        /// <param name="searchTerm">Search term to filter products</param>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 10)</param>
        /// <returns>Paged search results</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(PagedResult<ProductListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<ProductListDto>>> SearchProducts(
            [FromQuery] string searchTerm,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                _logger.LogInformation("Searching products with term: '{SearchTerm}', Page: {Page}, PageSize: {PageSize}", 
                    searchTerm, page, pageSize);
                
                var result = await _productService.SearchProductsAsync(searchTerm, page, pageSize);
                
                _logger.LogInformation("Found {Count} products out of {Total} matching search term", 
                    result.Items.Count(), result.TotalCount);
                
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid search parameters: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get a specific product by ID
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Product details</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductResponseDto>> GetProduct(Guid id)
        {
            try
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
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid product ID: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get a product by SKU
        /// </summary>
        /// <param name="sku">Product SKU</param>
        /// <returns>Product details</returns>
        [HttpGet("by-sku/{sku}")]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductResponseDto>> GetProductBySKU(string sku)
        {
            try
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
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid SKU: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get products by category with pagination
        /// </summary>
        /// <param name="category">Category name</param>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 10)</param>
        /// <returns>Paged products by category</returns>
        [HttpGet("category/{category}")]
        [ProducesResponseType(typeof(PagedResult<ProductListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<ProductListDto>>> GetProductsByCategory(
            string category,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                _logger.LogInformation("Getting products by category: {Category}", category);
                
                var result = await _productService.GetProductsByCategoryAsync(category, page, pageSize);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid category parameters: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get products by brand with pagination
        /// </summary>
        /// <param name="brand">Brand name</param>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 10)</param>
        /// <returns>Paged products by brand</returns>
        [HttpGet("brand/{brand}")]
        [ProducesResponseType(typeof(PagedResult<ProductListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<ProductListDto>>> GetProductsByBrand(
            string brand,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                _logger.LogInformation("Getting products by brand: {Brand}", brand);
                
                var result = await _productService.GetProductsByBrandAsync(brand, page, pageSize);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid brand parameters: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get products by price range with pagination
        /// </summary>
        /// <param name="minPrice">Minimum price</param>
        /// <param name="maxPrice">Maximum price</param>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 10)</param>
        /// <returns>Paged products within price range</returns>
        [HttpGet("price-range")]
        [ProducesResponseType(typeof(PagedResult<ProductListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<ProductListDto>>> GetProductsByPriceRange(
            [FromQuery] decimal minPrice,
            [FromQuery] decimal maxPrice,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                _logger.LogInformation("Getting products in price range: {MinPrice} - {MaxPrice}", minPrice, maxPrice);
                
                var result = await _productService.GetProductsByPriceRangeAsync(minPrice, maxPrice, page, pageSize);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid price range parameters: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get top-rated products
        /// </summary>
        /// <param name="minimumRating">Minimum rating (default: 4.0)</param>
        /// <param name="count">Number of products to return (default: 10)</param>
        /// <returns>Top-rated products</returns>
        [HttpGet("top-rated")]
        [ProducesResponseType(typeof(IEnumerable<ProductListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProductListDto>>> GetTopRatedProducts(
            [FromQuery] decimal minimumRating = 4.0m,
            [FromQuery] int count = 10)
        {
            try
            {
                _logger.LogInformation("Getting top {Count} products with minimum rating {MinRating}", count, minimumRating);
                
                var products = await _productService.GetTopRatedProductsAsync(minimumRating, count);
                return Ok(products);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid top-rated parameters: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        /// <param name="createDto">Product creation data</param>
        /// <returns>Created product</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductResponseDto>> CreateProduct(ProductCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Creating new product with SKU: {SKU}", createDto?.SKU);
                
                var product = await _productService.CreateProductAsync(createDto!);
                
                _logger.LogInformation("Product created successfully with ID: {ProductId}", product.Id);
                
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning("Invalid create request: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Conflict creating product: {Message}", ex.Message);
                return Conflict(ex.Message);
            }
        }

        /// <summary>
        /// Update an existing product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="updateDto">Product update data</param>
        /// <returns>Updated product</returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductResponseDto>> UpdateProduct(Guid id, ProductUpdateDto updateDto)
        {
            try
            {
                _logger.LogInformation("Updating product with ID: {ProductId}", id);
                
                var product = await _productService.UpdateProductAsync(id, updateDto);
                if (product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found for update", id);
                    return NotFound($"Product with ID {id} not found");
                }

                _logger.LogInformation("Product updated successfully with ID: {ProductId}", product.Id);
                
                return Ok(product);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid update request: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Conflict updating product: {Message}", ex.Message);
                return Conflict(ex.Message);
            }
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting product with ID: {ProductId}", id);
                
                var success = await _productService.DeleteProductAsync(id);
                if (!success)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found for deletion", id);
                    return NotFound($"Product with ID {id} not found");
                }

                _logger.LogInformation("Product deleted successfully with ID: {ProductId}", id);
                
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid delete request: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Create multiple products in bulk
        /// </summary>
        /// <param name="createDtos">Collection of product creation data</param>
        /// <returns>Number of products created</returns>
        [HttpPost("bulk")]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> BulkCreateProducts(IEnumerable<ProductCreateDto> createDtos)
        {
            try
            {
                _logger.LogInformation("Bulk creating {Count} products", createDtos?.Count() ?? 0);
                
                var count = await _productService.BulkCreateProductsAsync(createDtos!);
                
                _logger.LogInformation("Bulk created {Count} products successfully", count);
                
                return Created("", new { Message = $"Successfully created {count} products", Count = count });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid bulk create request: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Conflict in bulk create: {Message}", ex.Message);
                return Conflict(ex.Message);
            }
        }

        /// <summary>
        /// Generate multiple sample products for testing
        /// </summary>
        /// <param name="count">Number of products to generate</param>
        /// <returns>Number of products created</returns>
        [HttpPost("generate/{count:int}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<object>> GenerateProducts(int count)
        {
            try
            {
                _logger.LogInformation("Generating {Count} sample products", count);
                
                if (count <= 0 || count > 1000)
                {
                    return BadRequest("Count must be between 1 and 1000");
                }

                var createdCount = await _productService.GenerateProductsAsync(count);
                
                _logger.LogInformation("Successfully generated {CreatedCount} products", createdCount);
                
                return Ok(new { message = $"Successfully generated {createdCount} products", count = createdCount });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid generate request: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Check if a product exists
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Boolean indicating existence</returns>
        [HttpHead("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ProductExists(Guid id)
        {
            try
            {
                var exists = await _productService.ExistsAsync(id);
                return exists ? Ok() : NotFound();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid exists check: {Message}", ex.Message);
                return BadRequest();
            }
        }
    }
}
