using Microsoft.EntityFrameworkCore;
using BackendApi.Models;
using BackendApi.Repositories.Interfaces;

namespace BackendApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> UpdateAsync(Guid id, Product product)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
                return null;

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Category = product.Category;
            existingProduct.Brand = product.Brand;
            existingProduct.Price = product.Price;
            existingProduct.StockQuantity = product.StockQuantity;
            existingProduct.SKU = product.SKU;
            existingProduct.ReleaseDate = product.ReleaseDate;
            existingProduct.AvailabilityStatus = product.AvailabilityStatus;
            existingProduct.CustomerRating = product.CustomerRating;
            existingProduct.AvailableColors = product.AvailableColors;
            existingProduct.AvailableSizes = product.AvailableSizes;

            await _context.SaveChangesAsync();
            return existingProduct;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Products.AnyAsync(p => p.Id == id);
        }

        public async Task<Product?> GetBySKUAsync(string sku)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.SKU == sku);
        }
    }
}
