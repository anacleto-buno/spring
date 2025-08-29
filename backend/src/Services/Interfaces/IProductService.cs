using BackendApi.DTOs.Product;
using BackendApi.DTOs.Common;

namespace BackendApi.Services.Interfaces
{
    /// <summary>
    /// Product service interface defining business operations for products
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Gets a paged list of products with filtering options
        /// </summary>
        /// <param name="filter">Filter and pagination parameters</param>
        /// <returns>Paged result of products</returns>
        Task<PagedResult<ProductListDto>> GetProductsAsync(ProductFilterDto filter);

        /// <summary>
        /// Gets a product by its unique identifier
        /// </summary>
        /// <param name="id">Product identifier</param>
        /// <returns>Product details or null if not found</returns>
        Task<ProductResponseDto?> GetProductByIdAsync(Guid id);

        /// <summary>
        /// Gets a product by its SKU
        /// </summary>
        /// <param name="sku">Product SKU</param>
        /// <returns>Product details or null if not found</returns>
        Task<ProductResponseDto?> GetProductBySKUAsync(string sku);

        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="createDto">Product creation data</param>
        /// <returns>Created product details</returns>
        Task<ProductResponseDto> CreateProductAsync(ProductCreateDto createDto);

        /// <summary>
        /// Updates an existing product
        /// </summary>
        /// <param name="id">Product identifier</param>
        /// <param name="updateDto">Product update data</param>
        /// <returns>Updated product details or null if not found</returns>
        Task<ProductResponseDto?> UpdateProductAsync(Guid id, ProductUpdateDto updateDto);

        /// <summary>
        /// Deletes a product
        /// </summary>
        /// <param name="id">Product identifier</param>
        /// <returns>True if deleted, false if not found</returns>
        Task<bool> DeleteProductAsync(Guid id);

        /// <summary>
        /// Checks if a product exists
        /// </summary>
        /// <param name="id">Product identifier</param>
        /// <returns>True if exists, false otherwise</returns>
        Task<bool> ExistsAsync(Guid id);

        /// <summary>
        /// Searches products by term with pagination
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Paged search results</returns>
        Task<PagedResult<ProductListDto>> SearchProductsAsync(string searchTerm, int page = 1, int pageSize = 10);

        /// <summary>
        /// Gets products by category with pagination
        /// </summary>
        /// <param name="category">Category name</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Paged products by category</returns>
        Task<PagedResult<ProductListDto>> GetProductsByCategoryAsync(string category, int page = 1, int pageSize = 10);

        /// <summary>
        /// Gets products by brand with pagination
        /// </summary>
        /// <param name="brand">Brand name</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Paged products by brand</returns>
        Task<PagedResult<ProductListDto>> GetProductsByBrandAsync(string brand, int page = 1, int pageSize = 10);

        /// <summary>
        /// Gets products within a price range with pagination
        /// </summary>
        /// <param name="minPrice">Minimum price</param>
        /// <param name="maxPrice">Maximum price</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Paged products within price range</returns>
        Task<PagedResult<ProductListDto>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice, int page = 1, int pageSize = 10);

        /// <summary>
        /// Gets top-rated products
        /// </summary>
        /// <param name="minimumRating">Minimum rating threshold</param>
        /// <param name="count">Number of products to return</param>
        /// <returns>Top-rated products</returns>
        Task<IEnumerable<ProductListDto>> GetTopRatedProductsAsync(decimal minimumRating = 4.0m, int count = 10);

        /// <summary>
        /// Creates multiple products in bulk
        /// </summary>
        /// <param name="createDtos">Collection of product creation data</param>
        /// <returns>Number of products created</returns>
        Task<int> BulkCreateProductsAsync(IEnumerable<ProductCreateDto> createDtos);

        /// <summary>
        /// Generates sample products for testing
        /// </summary>
        /// <param name="count">Number of products to generate</param>
        /// <returns>Number of products created</returns>
        Task<int> GenerateProductsAsync(int count);
    }
}
