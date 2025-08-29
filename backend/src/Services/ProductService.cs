using AutoMapper;
using BackendApi.DTOs.Product;
using BackendApi.DTOs.Common;
using BackendApi.Models;
using BackendApi.Repositories.Interfaces;
using BackendApi.Services.Interfaces;

namespace BackendApi.Services
{
    /// <summary>
    /// Product service implementation with comprehensive business logic
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PagedResult<ProductListDto>> GetProductsAsync(ProductFilterDto filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            if (!filter.IsValid())
                throw new ArgumentException(filter.GetValidationError(), nameof(filter));

            var (products, totalCount) = await _unitOfWork.Products.GetPagedAsync(
                filter.Page,
                filter.PageSize,
                BuildFilterExpression(filter),
                BuildOrderExpression(filter));

            var productDtos = _mapper.Map<List<ProductListDto>>(products);

            return new PagedResult<ProductListDto>(productDtos, totalCount, filter.Page, filter.PageSize);
        }

        public async Task<ProductResponseDto?> GetProductByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Product ID cannot be empty", nameof(id));

            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                return null;

            return _mapper.Map<ProductResponseDto>(product);
        }

        public async Task<ProductResponseDto?> GetProductBySKUAsync(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
                throw new ArgumentException("SKU cannot be null or empty", nameof(sku));

            var product = await _unitOfWork.Products.GetBySKUAsync(sku);
            if (product == null)
                return null;

            return _mapper.Map<ProductResponseDto>(product);
        }

        public async Task<ProductResponseDto> CreateProductAsync(ProductCreateDto createDto)
        {
            if (createDto == null)
                throw new ArgumentNullException(nameof(createDto));

            // Check if SKU already exists
            if (await _unitOfWork.Products.ExistsBySKUAsync(createDto.SKU))
                throw new InvalidOperationException($"A product with SKU '{createDto.SKU}' already exists");

            var product = _mapper.Map<Product>(createDto);
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProductResponseDto>(product);
        }

        public async Task<ProductResponseDto?> UpdateProductAsync(Guid id, ProductUpdateDto updateDto)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Product ID cannot be empty", nameof(id));

            if (updateDto == null)
                throw new ArgumentNullException(nameof(updateDto));

            var existingProduct = await _unitOfWork.Products.GetByIdAsync(id);
            if (existingProduct == null)
                return null;

            // Check if SKU is being changed and if the new SKU already exists
            if (existingProduct.SKU != updateDto.SKU)
            {
                if (await _unitOfWork.Products.ExistsBySKUAsync(updateDto.SKU))
                    throw new InvalidOperationException($"A product with SKU '{updateDto.SKU}' already exists");
            }

            _mapper.Map(updateDto, existingProduct);
            existingProduct.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Products.Update(existingProduct);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProductResponseDto>(existingProduct);
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Product ID cannot be empty", nameof(id));

            var success = await _unitOfWork.Products.RemoveByIdAsync(id);
            if (success)
            {
                await _unitOfWork.SaveChangesAsync();
            }

            return success;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Product ID cannot be empty", nameof(id));

            return await _unitOfWork.Products.AnyAsync(p => p.Id == id);
        }

        public async Task<PagedResult<ProductListDto>> SearchProductsAsync(string searchTerm, int page = 1, int pageSize = 10)
        {
            if (page <= 0)
                throw new ArgumentException("Page must be greater than 0", nameof(page));

            if (pageSize <= 0 || pageSize > 100)
                throw new ArgumentException("Page size must be between 1 and 100", nameof(pageSize));

            var products = await _unitOfWork.Products.SearchAsync(searchTerm ?? "");
            var totalCount = products.Count();

            var pagedProducts = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var productDtos = _mapper.Map<IEnumerable<ProductListDto>>(pagedProducts);

            return new PagedResult<ProductListDto>(productDtos, totalCount, page, pageSize);
        }

        public async Task<PagedResult<ProductListDto>> GetProductsByCategoryAsync(string category, int page = 1, int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("Category cannot be null or empty", nameof(category));

            var (products, totalCount) = await _unitOfWork.Products.GetByCategoryAsync(category, page, pageSize);
            var productDtos = _mapper.Map<IEnumerable<ProductListDto>>(products);

            return new PagedResult<ProductListDto>(productDtos, totalCount, page, pageSize);
        }

        public async Task<PagedResult<ProductListDto>> GetProductsByBrandAsync(string brand, int page = 1, int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(brand))
                throw new ArgumentException("Brand cannot be null or empty", nameof(brand));

            var (products, totalCount) = await _unitOfWork.Products.GetByBrandAsync(brand, page, pageSize);
            var productDtos = _mapper.Map<IEnumerable<ProductListDto>>(products);

            return new PagedResult<ProductListDto>(productDtos, totalCount, page, pageSize);
        }

        public async Task<PagedResult<ProductListDto>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice, int page = 1, int pageSize = 10)
        {
            var (products, totalCount) = await _unitOfWork.Products.GetByPriceRangeAsync(minPrice, maxPrice, page, pageSize);
            var productDtos = _mapper.Map<IEnumerable<ProductListDto>>(products);

            return new PagedResult<ProductListDto>(productDtos, totalCount, page, pageSize);
        }

        public async Task<IEnumerable<ProductListDto>> GetTopRatedProductsAsync(decimal minimumRating = 4.0m, int count = 10)
        {
            var products = await _unitOfWork.Products.GetTopRatedAsync(minimumRating, count);
            return _mapper.Map<IEnumerable<ProductListDto>>(products);
        }

        public async Task<int> BulkCreateProductsAsync(IEnumerable<ProductCreateDto> createDtos)
        {
            if (createDtos == null || !createDtos.Any())
                throw new ArgumentException("Product list cannot be null or empty", nameof(createDtos));

            var products = _mapper.Map<IEnumerable<Product>>(createDtos);
            
            // Check for duplicate SKUs within the batch and against existing products
            var skus = products.Select(p => p.SKU).ToList();
            if (skus.Count != skus.Distinct().Count())
                throw new InvalidOperationException("Duplicate SKUs found in the batch");

            foreach (var sku in skus)
            {
                if (await _unitOfWork.Products.ExistsBySKUAsync(sku))
                    throw new InvalidOperationException($"A product with SKU '{sku}' already exists");
            }

            await _unitOfWork.Products.AddRangeAsync(products);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> GenerateProductsAsync(int count)
        {
            if (count <= 0)
                throw new ArgumentException("Count must be greater than zero", nameof(count));

            if (count > 1000)
                throw new ArgumentException("Count cannot exceed 1000", nameof(count));

            var products = BackendApi.Utilities.ProductGenerator.GenerateProducts(count);
            var productEntities = _mapper.Map<IEnumerable<Product>>(products);

            await _unitOfWork.Products.AddRangeAsync(productEntities);
            return await _unitOfWork.SaveChangesAsync();
        }

        private System.Linq.Expressions.Expression<Func<Product, bool>>? BuildFilterExpression(ProductFilterDto filter)
        {
            System.Linq.Expressions.Expression<Func<Product, bool>>? expression = null;

            if (!string.IsNullOrWhiteSpace(filter.Category))
            {
                expression = CombineExpressions(expression, p => p.Category != null && p.Category.ToLower() == filter.Category.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(filter.Brand))
            {
                expression = CombineExpressions(expression, p => p.Brand != null && p.Brand.ToLower() == filter.Brand.ToLower());
            }

            if (filter.MinPrice.HasValue)
            {
                expression = CombineExpressions(expression, p => p.Price >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                expression = CombineExpressions(expression, p => p.Price <= filter.MaxPrice.Value);
            }

            if (filter.MinRating.HasValue)
            {
                expression = CombineExpressions(expression, p => p.CustomerRating.HasValue && p.CustomerRating >= filter.MinRating.Value);
            }

            if (filter.InStockOnly.HasValue && filter.InStockOnly.Value)
            {
                expression = CombineExpressions(expression, p => p.StockQuantity > 0);
            }

            if (!string.IsNullOrWhiteSpace(filter.AvailabilityStatus))
            {
                expression = CombineExpressions(expression, p => p.AvailabilityStatus != null && p.AvailabilityStatus.ToLower() == filter.AvailabilityStatus.ToLower());
            }

            if (filter.ReleaseDateFrom.HasValue)
            {
                expression = CombineExpressions(expression, p => p.ReleaseDate >= filter.ReleaseDateFrom.Value);
            }

            if (filter.ReleaseDateTo.HasValue)
            {
                expression = CombineExpressions(expression, p => p.ReleaseDate <= filter.ReleaseDateTo.Value);
            }

            return expression;
        }

        private Func<IQueryable<Product>, IOrderedQueryable<Product>>? BuildOrderExpression(ProductFilterDto filter)
        {
            if (string.IsNullOrWhiteSpace(filter.SortBy))
                return q => q.OrderBy(p => p.Name);

            return filter.SortBy.ToLower() switch
            {
                "name" => filter.IsDescending ? q => q.OrderByDescending(p => p.Name) : q => q.OrderBy(p => p.Name),
                "price" => filter.IsDescending ? q => q.OrderByDescending(p => p.Price) : q => q.OrderBy(p => p.Price),
                "category" => filter.IsDescending ? q => q.OrderByDescending(p => p.Category) : q => q.OrderBy(p => p.Category),
                "brand" => filter.IsDescending ? q => q.OrderByDescending(p => p.Brand) : q => q.OrderBy(p => p.Brand),
                "rating" => filter.IsDescending ? q => q.OrderByDescending(p => p.CustomerRating) : q => q.OrderBy(p => p.CustomerRating),
                "releasedate" => filter.IsDescending ? q => q.OrderByDescending(p => p.ReleaseDate) : q => q.OrderBy(p => p.ReleaseDate),
                "stock" => filter.IsDescending ? q => q.OrderByDescending(p => p.StockQuantity) : q => q.OrderBy(p => p.StockQuantity),
                _ => q => q.OrderBy(p => p.Name)
            };
        }

        private System.Linq.Expressions.Expression<Func<Product, bool>>? CombineExpressions(
            System.Linq.Expressions.Expression<Func<Product, bool>>? first,
            System.Linq.Expressions.Expression<Func<Product, bool>> second)
        {
            if (first == null)
                return second;

            var parameter = System.Linq.Expressions.Expression.Parameter(typeof(Product), "p");
            var firstBody = first.Body.ReplaceParameter(first.Parameters[0], parameter);
            var secondBody = second.Body.ReplaceParameter(second.Parameters[0], parameter);
            var combined = System.Linq.Expressions.Expression.AndAlso(firstBody, secondBody);

            return System.Linq.Expressions.Expression.Lambda<Func<Product, bool>>(combined, parameter);
        }
    }

    public static class ExpressionExtensions
    {
        public static System.Linq.Expressions.Expression ReplaceParameter(
            this System.Linq.Expressions.Expression expression,
            System.Linq.Expressions.ParameterExpression source,
            System.Linq.Expressions.Expression target)
        {
            return new ParameterReplacer { Source = source, Target = target }.Visit(expression);
        }

        private class ParameterReplacer : System.Linq.Expressions.ExpressionVisitor
        {
            public System.Linq.Expressions.ParameterExpression? Source { get; set; }
            public System.Linq.Expressions.Expression? Target { get; set; }

            protected override System.Linq.Expressions.Expression VisitParameter(System.Linq.Expressions.ParameterExpression node)
            {
                return node == Source && Target != null ? Target : base.VisitParameter(node);
            }
        }
    }
}
