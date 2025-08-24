using Xunit;
using FluentAssertions;
using BackendApi.Repositories;
using BackendApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

public class ProductRepositoryTests
{
    private DbContextOptions<AppDbContext> GetDbOptions()
    {
        // Use a unique DB name per test to avoid data leakage
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task SearchAsync_FindsProductsByNameOrDescription()
    {
        // Arrange
        var options = GetDbOptions();
        using var context = new AppDbContext(options);
        context.Products.AddRange(new List<Product>
        {
            new Product { Name = "Apple iPhone", Description = "Smartphone", SKU = "A123" },
            new Product { Name = "Samsung Galaxy", Description = "Android Phone", SKU = "S456" },
            new Product { Name = "Apple Watch", Description = "Smartwatch", SKU = "A789" }
        });
        await context.SaveChangesAsync();
        var repo = new ProductRepository(context);

        // Act
        var results = await repo.SearchAsync("Apple");

        // Assert
        results.Should().HaveCount(2);
        results.Should().AllSatisfy(p => p.Name.Contains("Apple"));
    }

    [Fact]
    public async Task SearchAsync_EmptyTerm_ReturnsAllProducts()
    {
        // Arrange
        var options = GetDbOptions();
        using var context = new AppDbContext(options);
        context.Products.AddRange(new List<Product>
        {
            new Product { Name = "Apple iPhone", Description = "Smartphone", SKU = "A123" },
            new Product { Name = "Samsung Galaxy", Description = "Android Phone", SKU = "S456" }
        });
        await context.SaveChangesAsync();
        var repo = new ProductRepository(context);

        // Act
        var results = await repo.SearchAsync("");

        // Assert
        results.Should().HaveCount(2);
    }

    [Fact]
    public async Task SearchAsync_ThrowsException_Propagates()
    {
        var options = GetDbOptions();
        using var context = new AppDbContext(options);
        var repo = new ProductRepository(context);
        // Simulate exception by disposing context
        context.Dispose();
        Func<Task> act = async () => await repo.SearchAsync("Apple");
        await act.Should().ThrowAsync<ObjectDisposedException>();
    }
}
