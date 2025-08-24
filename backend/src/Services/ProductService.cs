using BackendApi.DTOs;
using BackendApi.Models;
using BackendApi.Repositories.Interfaces;
using BackendApi.Services.Interfaces;

namespace BackendApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Category = p.Category,
                Brand = p.Brand,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                SKU = p.SKU,
                ReleaseDate = p.ReleaseDate,
                AvailabilityStatus = p.AvailabilityStatus,
                CustomerRating = p.CustomerRating,
                AvailableColors = p.AvailableColors,
                AvailableSizes = p.AvailableSizes
            });
        }

        public async Task<ProductDto?> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category,
                Brand = product.Brand,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                SKU = product.SKU,
                ReleaseDate = product.ReleaseDate,
                AvailabilityStatus = product.AvailabilityStatus,
                CustomerRating = product.CustomerRating,
                AvailableColors = product.AvailableColors,
                AvailableSizes = product.AvailableSizes
            };
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Category = createProductDto.Category,
                Brand = createProductDto.Brand,
                Price = createProductDto.Price,
                StockQuantity = createProductDto.StockQuantity,
                SKU = createProductDto.SKU,
                ReleaseDate = createProductDto.ReleaseDate,
                AvailabilityStatus = createProductDto.AvailabilityStatus,
                CustomerRating = createProductDto.CustomerRating,
                AvailableColors = createProductDto.AvailableColors,
                AvailableSizes = createProductDto.AvailableSizes
            };

            var createdProduct = await _productRepository.CreateAsync(product);
            
            return new ProductDto
            {
                Id = createdProduct.Id,
                Name = createdProduct.Name,
                Description = createdProduct.Description,
                Category = createdProduct.Category,
                Brand = createdProduct.Brand,
                Price = createdProduct.Price,
                StockQuantity = createdProduct.StockQuantity,
                SKU = createdProduct.SKU,
                ReleaseDate = createdProduct.ReleaseDate,
                AvailabilityStatus = createdProduct.AvailabilityStatus,
                CustomerRating = createdProduct.CustomerRating,
                AvailableColors = createdProduct.AvailableColors,
                AvailableSizes = createdProduct.AvailableSizes
            };
        }

        public async Task<ProductDto?> UpdateProductAsync(Guid id, UpdateProductDto updateProductDto)
        {
            var productToUpdate = new Product
            {
                Id = id,
                Name = updateProductDto.Name,
                Description = updateProductDto.Description,
                Category = updateProductDto.Category,
                Brand = updateProductDto.Brand,
                Price = updateProductDto.Price,
                StockQuantity = updateProductDto.StockQuantity,
                SKU = updateProductDto.SKU,
                ReleaseDate = updateProductDto.ReleaseDate,
                AvailabilityStatus = updateProductDto.AvailabilityStatus,
                CustomerRating = updateProductDto.CustomerRating,
                AvailableColors = updateProductDto.AvailableColors,
                AvailableSizes = updateProductDto.AvailableSizes
            };

            var updatedProduct = await _productRepository.UpdateAsync(id, productToUpdate);
            if (updatedProduct == null)
                return null;

            return new ProductDto
            {
                Id = updatedProduct.Id,
                Name = updatedProduct.Name,
                Description = updatedProduct.Description,
                Category = updatedProduct.Category,
                Brand = updatedProduct.Brand,
                Price = updatedProduct.Price,
                StockQuantity = updatedProduct.StockQuantity,
                SKU = updatedProduct.SKU,
                ReleaseDate = updatedProduct.ReleaseDate,
                AvailabilityStatus = updatedProduct.AvailabilityStatus,
                CustomerRating = updatedProduct.CustomerRating,
                AvailableColors = updatedProduct.AvailableColors,
                AvailableSizes = updatedProduct.AvailableSizes
            };
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            return await _productRepository.DeleteAsync(id);
        }

        public async Task<bool> ProductExistsAsync(Guid id)
        {
            return await _productRepository.ExistsAsync(id);
        }

        public async Task<ProductDto?> GetProductBySKUAsync(string sku)
        {
            var product = await _productRepository.GetBySKUAsync(sku);
            if (product == null)
                return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category,
                Brand = product.Brand,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                SKU = product.SKU,
                ReleaseDate = product.ReleaseDate,
                AvailabilityStatus = product.AvailabilityStatus,
                CustomerRating = product.CustomerRating,
                AvailableColors = product.AvailableColors,
                AvailableSizes = product.AvailableSizes
            };
        }
    }
}
