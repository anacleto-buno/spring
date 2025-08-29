using Xunit;
using FluentAssertions;
using FakeItEasy;
using AutoMapper;
using BackendApi.Services;
using BackendApi.Services.Interfaces;
using BackendApi.Repositories.Interfaces;
using BackendApi.DTOs.Product;
using BackendApi.DTOs.Common;
using BackendApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ProductServiceTests
{
    [Fact]
    public async Task GetProductsAsync_ReturnsPagedResults()
    {
        // Arrange
        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        var fakeMapper = A.Fake<IMapper>();
        var fakeRepo = A.Fake<IProductRepository>();
        
        var products = new List<Product>
        {
            new Product { Name = "Apple iPhone", Description = "Smartphone", SKU = "A123", Price = 999.99m },
            new Product { Name = "Samsung Galaxy", Description = "Android Phone", SKU = "S456", Price = 799.99m }
        };
        
        var productListDtos = new List<ProductListDto>
        {
            new ProductListDto { Name = "Apple iPhone", SKU = "A123", Price = 999.99m },
            new ProductListDto { Name = "Samsung Galaxy", SKU = "S456", Price = 799.99m }
        };
        
        A.CallTo(() => fakeUnitOfWork.Products).Returns(fakeRepo);
        A.CallTo(() => fakeRepo.GetPagedAsync(A<int>.Ignored, A<int>.Ignored, A<System.Linq.Expressions.Expression<Func<Product, bool>>>.Ignored, A<Func<IQueryable<Product>, IOrderedQueryable<Product>>>.Ignored, A<string>.Ignored))
            .WithAnyArguments()
            .Returns(Task.FromResult((products.AsEnumerable(), 2)));
        A.CallTo(() => fakeMapper.Map<List<ProductListDto>>(A<IEnumerable<Product>>.Ignored)).Returns(productListDtos);
        
        var service = new ProductService(fakeUnitOfWork, fakeMapper);
        var filter = new ProductFilterDto { Page = 1, PageSize = 10 };

        // Act
        var result = await service.GetProductsAsync(filter);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
    }

    [Fact]
    public async Task GetProductByIdAsync_ExistingId_ReturnsProduct()
    {
        // Arrange
        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        var fakeMapper = A.Fake<IMapper>();
        var fakeRepo = A.Fake<IProductRepository>();
        
        var product = new Product { Name = "Apple iPhone", Description = "Smartphone", SKU = "A123", Price = 999.99m };
        var productDto = new ProductResponseDto { Name = "Apple iPhone", SKU = "A123", Price = 999.99m };
        
        A.CallTo(() => fakeUnitOfWork.Products).Returns(fakeRepo);
        A.CallTo(() => fakeRepo.GetByIdAsync(A<System.Guid>.Ignored)).Returns(Task.FromResult<Product?>(product));
        A.CallTo(() => fakeMapper.Map<ProductResponseDto>(product)).Returns(productDto);
        
        var service = new ProductService(fakeUnitOfWork, fakeMapper);

        // Act
        var result = await service.GetProductByIdAsync(System.Guid.NewGuid());

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Apple iPhone");
    }
}
