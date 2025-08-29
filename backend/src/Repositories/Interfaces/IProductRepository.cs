using BackendApi.Models;

namespace BackendApi.Repositories.Interfaces
{
    /// <summary>
    /// Product repository interface with specific product operations
    /// </summary>
    public interface IProductRepository : IRepository<Product>
    {
        /// <summary>
        /// Gets a product by its SKU
        /// </summary>
        /// <param name="sku">The product SKU</param>
        /// <returns>The product if found, null otherwise</returns>
        Task<Product?> GetBySKUAsync(string sku);

        /// <summary>
        /// Searches products by term in name, description, category, or brand
        /// </summary>
        /// <param name="searchTerm">The search term</param>
        /// <returns>Collection of matching products</returns>
        Task<IEnumerable<Product>> SearchAsync(string searchTerm);

        /// <summary>
        /// Gets products by category with pagination
        /// </summary>
        /// <param name="category">The category to filter by</param>
        /// <param name="page">Page number (1-based)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <returns>Paged result of products</returns>
        Task<(IEnumerable<Product> Items, int TotalCount)> GetByCategoryAsync(string category, int page, int pageSize);

        /// <summary>
        /// Gets products by brand with pagination
        /// </summary>
        /// <param name="brand">The brand to filter by</param>
        /// <param name="page">Page number (1-based)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <returns>Paged result of products</returns>
        Task<(IEnumerable<Product> Items, int TotalCount)> GetByBrandAsync(string brand, int page, int pageSize);

        /// <summary>
        /// Gets products within a price range
        /// </summary>
        /// <param name="minPrice">Minimum price</param>
        /// <param name="maxPrice">Maximum price</param>
        /// <param name="page">Page number (1-based)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <returns>Paged result of products</returns>
        Task<(IEnumerable<Product> Items, int TotalCount)> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice, int page, int pageSize);

        /// <summary>
        /// Checks if a product with the given SKU exists
        /// </summary>
        /// <param name="sku">The SKU to check</param>
        /// <returns>True if exists, false otherwise</returns>
        Task<bool> ExistsBySKUAsync(string sku);

        /// <summary>
        /// Gets top-rated products
        /// </summary>
        /// <param name="minimumRating">Minimum rating threshold</param>
        /// <param name="count">Number of products to return</param>
        /// <returns>Collection of top-rated products</returns>
        Task<IEnumerable<Product>> GetTopRatedAsync(decimal minimumRating, int count);
    }
}
