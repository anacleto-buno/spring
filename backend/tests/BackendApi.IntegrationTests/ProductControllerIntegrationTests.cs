using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using BackendApi.DTOs;
using BackendApi.Models;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace BackendApi.IntegrationTests;

public class ProductControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly HttpClient _httpClient;
    private readonly CustomWebApplicationFactory _factory;

    public ProductControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        // Reset database before each test
        await _factory.ResetDatabaseAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public async Task GetAllProducts_ReturnsEmptyList_WhenNoProductsExist()
    {
        // Act
        var response = await _httpClient.GetAsync("/api/product");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        products.Should().NotBeNull();
        products.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateProduct_ReturnsCreatedProduct()
    {
        // Arrange
        var newProduct = new ProductDto
        {
            Name = "Test Product",
            Description = "Test Description",
            Category = "Electronics",
            Brand = "TestBrand",
            Price = 99.99m,
            StockQuantity = 10,
            SKU = "TEST-001",
            ReleaseDate = DateTime.Now,
            AvailabilityStatus = "Available",
            CustomerRating = 4.5m,
            AvailableColors = "Red, Blue",
            AvailableSizes = "M, L"
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/product", newProduct);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdProduct = await response.Content.ReadFromJsonAsync<ProductDto>();
        createdProduct.Should().NotBeNull();
        createdProduct!.Name.Should().Be("Test Product");
        createdProduct.Price.Should().Be(99.99m);
        createdProduct.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetProductById_ReturnsProduct_WhenProductExists()
    {
        // Arrange - Create a product first
        var newProduct = new ProductDto
        {
            Name = "Test Product for Get",
            Description = "Test Description",
            Category = "Electronics",
            Brand = "TestBrand",
            Price = 199.99m,
            StockQuantity = 5,
            SKU = "TEST-002",
            ReleaseDate = DateTime.Now,
            AvailabilityStatus = "Available"
        };

        var createResponse = await _httpClient.PostAsJsonAsync("/api/product", newProduct);
        var createdProduct = await createResponse.Content.ReadFromJsonAsync<ProductDto>();

        // Act
        var response = await _httpClient.GetAsync($"/api/product/{createdProduct!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var retrievedProduct = await response.Content.ReadFromJsonAsync<ProductDto>();
        retrievedProduct.Should().NotBeNull();
        retrievedProduct!.Id.Should().Be(createdProduct.Id);
        retrievedProduct.Name.Should().Be("Test Product for Get");
    }

    [Fact]
    public async Task GetProductById_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _httpClient.GetAsync($"/api/product/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task SearchProducts_ReturnsFilteredResults()
    {
        // Arrange - Create test products
        var products = new[]
        {
            new ProductDto { Name = "Apple iPhone", Description = "Smartphone", SKU = "APPLE-001", Category = "Electronics", Brand = "Apple", Price = 999.99m, StockQuantity = 10, AvailabilityStatus = "Available" },
            new ProductDto { Name = "Samsung Galaxy", Description = "Android Phone", SKU = "SAMSUNG-001", Category = "Electronics", Brand = "Samsung", Price = 799.99m, StockQuantity = 15, AvailabilityStatus = "Available" },
            new ProductDto { Name = "Apple Watch", Description = "Smartwatch", SKU = "APPLE-002", Category = "Electronics", Brand = "Apple", Price = 399.99m, StockQuantity = 20, AvailabilityStatus = "Available" }
        };

        foreach (var product in products)
        {
            await _httpClient.PostAsJsonAsync("/api/product", product);
        }

        // Act
        var response = await _httpClient.GetAsync("/api/product/search?searchTerm=Apple");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var searchResults = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        searchResults.Should().NotBeNull();
        searchResults.Should().HaveCount(2);
        searchResults!.Should().OnlyContain(p => p.Name.Contains("Apple") || p.Brand.Contains("Apple"));
    }

    [Fact]
    public async Task SearchProducts_ReturnsBadRequest_WhenSearchTermIsEmpty()
    {
        // Arrange - Ensure we have some products
        await _httpClient.PostAsJsonAsync("/api/product", new CreateProductDto 
        { 
            Name = "Test Product", 
            SKU = "TEST-SEARCH", 
            Category = "Test", 
            Brand = "Test", 
            Price = 10.00m, 
            StockQuantity = 1,
            AvailabilityStatus = "Available",
            Description = "Test description",
            CustomerRating = 4.0m,
            ReleaseDate = DateTime.Now
        });

        // Act
        var response = await _httpClient.GetAsync("/api/product/search?searchTerm=");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateProduct_ReturnsNoContent()
    {
        // Arrange - Create a product first
        var newProduct = new CreateProductDto
        {
            Name = "Original Product",
            Description = "Original Description",
            Category = "Electronics",
            Brand = "TestBrand",
            Price = 99.99m,
            StockQuantity = 10,
            SKU = "UPDATE-001",
            AvailabilityStatus = "Available",
            CustomerRating = 4.0m,
            ReleaseDate = DateTime.Now
        };

        var createResponse = await _httpClient.PostAsJsonAsync("/api/product", newProduct);
        var createdProduct = await createResponse.Content.ReadFromJsonAsync<ProductDto>();

        // Create update DTO with all required fields
        var updateDto = new UpdateProductDto
        {
            Name = "Updated Product",
            Price = 149.99m,
            Description = "Updated Description",
            Category = "Electronics",
            Brand = "TestBrand",
            StockQuantity = 15,
            SKU = "UPDATE-001-MODIFIED",
            AvailabilityStatus = "Available",
            ReleaseDate = DateTime.Now,
            CustomerRating = 4.5m
        };

        // Act
        var response = await _httpClient.PutAsJsonAsync($"/api/product/{createdProduct!.Id}", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteProduct_ReturnsNoContent_WhenProductExists()
    {
        // Arrange - Create a product first
        var newProduct = new CreateProductDto
        {
            Name = "Product to Delete",
            SKU = "DELETE-001",
            Category = "Test",
            Brand = "Test",
            Price = 50.00m,
            StockQuantity = 1,
            AvailabilityStatus = "Available",
            Description = "Test product for deletion",
            CustomerRating = 4.0m,
            ReleaseDate = DateTime.Now
        };

        var createResponse = await _httpClient.PostAsJsonAsync("/api/product", newProduct);
        var createdProduct = await createResponse.Content.ReadFromJsonAsync<ProductDto>();

        // Act
        var response = await _httpClient.DeleteAsync($"/api/product/{createdProduct!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify product is deleted
        var getResponse = await _httpClient.GetAsync($"/api/product/{createdProduct.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteProduct_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Arrange - Use a non-existent product ID
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _httpClient.DeleteAsync($"/api/product/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GenerateProducts_CreatesMultipleProducts()
    {
        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/product/generate/5", (object?)null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        // The generate endpoint returns metadata, not a list of products
        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().NotBeNullOrEmpty();

        // Verify products were actually created in the database
        var allProductsResponse = await _httpClient.GetAsync("/api/product");
        allProductsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var allProducts = await allProductsResponse.Content.ReadFromJsonAsync<List<ProductDto>>();
        allProducts.Should().NotBeNull();
        allProducts.Should().HaveCountGreaterOrEqualTo(5);
    }

    [Fact]
    public async Task HealthCheck_ReturnsHealthy()
    {
        // Act
        var response = await _httpClient.GetAsync("/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
