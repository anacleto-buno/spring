using Xunit;
using FluentAssertions;
using FakeItEasy;
using BackendApi.Services;
using BackendApi.Services.Interfaces;
using BackendApi.Repositories.Interfaces;
using BackendApi.DTOs;
using BackendApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductServiceTests
{
    [Fact]
    public async Task SearchProductsAsync_ReturnsMatchingProducts()
    {
        // Arrange
        var fakeRepo = A.Fake<IProductRepository>();
        var products = new List<Product>
        {
            new Product { Name = "Apple iPhone", Description = "Smartphone", SKU = "A123" },
            new Product { Name = "Samsung Galaxy", Description = "Android Phone", SKU = "S456" },
            new Product { Name = "Apple Watch", Description = "Smartwatch", SKU = "A789" }
        };
        A.CallTo(() => fakeRepo.SearchAsync("Apple")).Returns(Task.FromResult<IEnumerable<Product>>(products.FindAll(p => p.Name.Contains("Apple"))));
        var service = new ProductService(fakeRepo);

        // Act
        var result = await service.SearchProductsAsync("Apple");

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().Contain(r => r.Name.Contains("Apple"));
    }

    [Fact]
    public async Task SearchProductsAsync_EmptyTerm_ReturnsAllProducts()
    {
        // Arrange
        var fakeRepo = A.Fake<IProductRepository>();
        var products = new List<Product>
        {
            new Product { Name = "Apple iPhone", Description = "Smartphone", SKU = "A123" },
            new Product { Name = "Samsung Galaxy", Description = "Android Phone", SKU = "S456" }
        };
        A.CallTo(() => fakeRepo.SearchAsync("")).Returns(Task.FromResult<IEnumerable<Product>>(products));
        var service = new ProductService(fakeRepo);

        // Act
        var result = await service.SearchProductsAsync("");

        // Assert
        result.Should().HaveCount(2);
    }
}
