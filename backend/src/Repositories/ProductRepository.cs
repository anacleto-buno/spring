using Microsoft.EntityFrameworkCore;
using BackendApi.Models;
using BackendApi.Repositories.Interfaces;

namespace BackendApi.Repositories
{
    /// <summary>
    /// Product repository implementation with specific product operations
    /// </summary>
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Product?> GetBySKUAsync(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
                throw new ArgumentException("SKU cannot be null or empty", nameof(sku));

            return await _dbSet.FirstOrDefaultAsync(p => p.SKU == sku);
        }

        public async Task<IEnumerable<Product>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            var lowerSearchTerm = searchTerm.ToLower();

            // Detect provider: use ILike for PostgreSQL, fallback for other providers
            var providerName = _context.Database.ProviderName;
            if (providerName != null && providerName.Contains("Npgsql"))
            {
                return await _dbSet
                    .Where(p =>
                        EF.Functions.ILike(p.Name, $"%{searchTerm}%") ||
                        EF.Functions.ILike(p.Description ?? "", $"%{searchTerm}%") ||
                        EF.Functions.ILike(p.Category ?? "", $"%{searchTerm}%") ||
                        EF.Functions.ILike(p.Brand ?? "", $"%{searchTerm}%") ||
                        EF.Functions.ILike(p.SKU, $"%{searchTerm}%") ||
                        EF.Functions.ILike(p.AvailabilityStatus ?? "", $"%{searchTerm}%") ||
                        (p.AvailableColors != null && EF.Functions.ILike(p.AvailableColors, $"%{searchTerm}%")) ||
                        (p.AvailableSizes != null && EF.Functions.ILike(p.AvailableSizes, $"%{searchTerm}%"))
                    )
                    .OrderBy(p => p.Name)
                    .ToListAsync();
            }
            else
            {
                return await _dbSet
                    .Where(p =>
                        p.Name.ToLower().Contains(lowerSearchTerm) ||
                        (p.Description != null && p.Description.ToLower().Contains(lowerSearchTerm)) ||
                        (p.Category != null && p.Category.ToLower().Contains(lowerSearchTerm)) ||
                        (p.Brand != null && p.Brand.ToLower().Contains(lowerSearchTerm)) ||
                        p.SKU.ToLower().Contains(lowerSearchTerm) ||
                        (p.AvailabilityStatus != null && p.AvailabilityStatus.ToLower().Contains(lowerSearchTerm)) ||
                        (p.AvailableColors != null && p.AvailableColors.ToLower().Contains(lowerSearchTerm)) ||
                        (p.AvailableSizes != null && p.AvailableSizes.ToLower().Contains(lowerSearchTerm))
                    )
                    .OrderBy(p => p.Name)
                    .ToListAsync();
            }
        }

        public async Task<(IEnumerable<Product> Items, int TotalCount)> GetByCategoryAsync(string category, int page, int pageSize)
        {
            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("Category cannot be null or empty", nameof(category));

            return await GetPagedAsync(
                page,
                pageSize,
                filter: p => p.Category != null && p.Category.ToLower() == category.ToLower(),
                orderBy: q => q.OrderBy(p => p.Name));
        }

        public async Task<(IEnumerable<Product> Items, int TotalCount)> GetByBrandAsync(string brand, int page, int pageSize)
        {
            if (string.IsNullOrWhiteSpace(brand))
                throw new ArgumentException("Brand cannot be null or empty", nameof(brand));

            return await GetPagedAsync(
                page,
                pageSize,
                filter: p => p.Brand != null && p.Brand.ToLower() == brand.ToLower(),
                orderBy: q => q.OrderBy(p => p.Name));
        }

        public async Task<(IEnumerable<Product> Items, int TotalCount)> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice, int page, int pageSize)
        {
            if (minPrice < 0)
                throw new ArgumentException("Minimum price cannot be negative", nameof(minPrice));

            if (maxPrice < minPrice)
                throw new ArgumentException("Maximum price cannot be less than minimum price", nameof(maxPrice));

            return await GetPagedAsync(
                page,
                pageSize,
                filter: p => p.Price >= minPrice && p.Price <= maxPrice,
                orderBy: q => q.OrderBy(p => p.Price));
        }

        public async Task<bool> ExistsBySKUAsync(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
                throw new ArgumentException("SKU cannot be null or empty", nameof(sku));

            return await AnyAsync(p => p.SKU == sku);
        }

        public async Task<IEnumerable<Product>> GetTopRatedAsync(decimal minimumRating, int count)
        {
            if (minimumRating < 0 || minimumRating > 5)
                throw new ArgumentException("Rating must be between 0 and 5", nameof(minimumRating));

            if (count <= 0)
                throw new ArgumentException("Count must be greater than 0", nameof(count));

            return await _dbSet
                .Where(p => p.CustomerRating.HasValue && p.CustomerRating >= minimumRating)
                .OrderByDescending(p => p.CustomerRating)
                .ThenBy(p => p.Name)
                .Take(count)
                .ToListAsync();
        }
    }
}
