using BackendApi.DTOs;

namespace BackendApi.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(Guid id);
        Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
        Task<ProductDto?> UpdateProductAsync(Guid id, UpdateProductDto updateProductDto);
        Task<bool> DeleteProductAsync(Guid id);
        Task<bool> ProductExistsAsync(Guid id);
        Task<ProductDto?> GetProductBySKUAsync(string sku);
    }
}
