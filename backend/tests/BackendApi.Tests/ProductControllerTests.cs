using Xunit;
using FluentAssertions;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BackendApi.Controllers;
using BackendApi.Services.Interfaces;
using BackendApi.DTOs.Product;
using BackendApi.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ProductControllerTests
{
    private BackendApi.Controllers.ProductController CreateController(IProductService? service = null)
    {
        var fakeService = service ?? A.Fake<IProductService>();
        var fakeLogger = A.Fake<ILogger<BackendApi.Controllers.ProductController>>();
        return new BackendApi.Controllers.ProductController(fakeService, fakeLogger);
    }

    [Fact]
    public async Task GetProducts_ReturnsOkWithProducts()
    {
        // Arrange
        var fakeService = A.Fake<IProductService>();
        var pagedResult = new PagedResult<ProductListDto>(
            new[] { new ProductListDto { Name = "Test" } }, 
            1, 1, 10);
        A.CallTo(() => fakeService.GetProductsAsync(A<ProductFilterDto>.Ignored))
            .Returns(Task.FromResult(pagedResult));
        var controller = CreateController(fakeService);
        var filter = new ProductFilterDto();

        // Act
        var result = await controller.GetProducts(filter);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result.Result!;
        okResult.Value.Should().BeOfType<PagedResult<ProductListDto>>();
    }

    [Fact]
    public async Task GetProduct_ReturnsNotFound_WhenMissing()
    {
        // Arrange
        var fakeService = A.Fake<IProductService>();
        A.CallTo(() => fakeService.GetProductByIdAsync(A<Guid>.Ignored))
            .Returns(Task.FromResult<ProductResponseDto?>(null));
        var controller = CreateController(fakeService);

        // Act
        var result = await controller.GetProduct(Guid.NewGuid());

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task SearchProducts_ReturnsOk_WhenTermIsValid()
    {
        // Arrange
        var fakeService = A.Fake<IProductService>();
        var pagedResult = new PagedResult<ProductListDto>(
            new[] { new ProductListDto { Name = "Apple" } }, 
            1, 1, 10);
        A.CallTo(() => fakeService.SearchProductsAsync("apple", 1, 10))
            .Returns(Task.FromResult(pagedResult));
        var controller = CreateController(fakeService);

        // Act
        var result = await controller.SearchProducts("apple", 1, 10);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task CreateProduct_ReturnsBadRequest_WhenModelInvalid()
    {
        // Arrange
        var controller = CreateController();
        controller.ModelState.AddModelError("Name", "Required");

        // Act
        var result = await controller.CreateProduct(new ProductCreateDto());

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task CreateProduct_ReturnsCreated_WhenValid()
    {
        // Arrange
        var fakeService = A.Fake<IProductService>();
        var dto = new ProductCreateDto { Name = "Test", SKU = "TEST-001", Price = 10.00m };
        var created = new ProductResponseDto { Id = Guid.NewGuid(), Name = "Test", SKU = "TEST-001", Price = 10.00m };
        A.CallTo(() => fakeService.CreateProductAsync(dto))
            .Returns(Task.FromResult(created));
        var controller = CreateController(fakeService);

        // Act
        var result = await controller.CreateProduct(dto);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    public async Task UpdateProduct_ReturnsNotFound_WhenMissing()
    {
        // Arrange
        var fakeService = A.Fake<IProductService>();
        A.CallTo(() => fakeService.UpdateProductAsync(A<Guid>.Ignored, A<ProductUpdateDto>.Ignored))
            .Returns(Task.FromResult<ProductResponseDto?>(null));
        var controller = CreateController(fakeService);

        // Act
        var result = await controller.UpdateProduct(Guid.NewGuid(), new ProductUpdateDto());

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task DeleteProduct_ReturnsNotFound_WhenMissing()
    {
        // Arrange
        var fakeService = A.Fake<IProductService>();
        A.CallTo(() => fakeService.DeleteProductAsync(A<Guid>.Ignored))
            .Returns(Task.FromResult(false));
        var controller = CreateController(fakeService);

        // Act
        var result = await controller.DeleteProduct(Guid.NewGuid());

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task DeleteProduct_ReturnsNoContent_WhenDeleted()
    {
        // Arrange
        var fakeService = A.Fake<IProductService>();
        A.CallTo(() => fakeService.DeleteProductAsync(A<Guid>.Ignored))
            .Returns(Task.FromResult(true));
        var controller = CreateController(fakeService);

        // Act
        var result = await controller.DeleteProduct(Guid.NewGuid());

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task GetProductsByCategory_ReturnsOkWithProducts()
    {
        // Arrange
        var fakeService = A.Fake<IProductService>();
        var pagedResult = new PagedResult<ProductListDto>(
            new[] { new ProductListDto { Name = "Test", Category = "Electronics" } }, 
            1, 1, 10);
        A.CallTo(() => fakeService.GetProductsByCategoryAsync("Electronics", 1, 10))
            .Returns(Task.FromResult(pagedResult));
        var controller = CreateController(fakeService);

        // Act
        var result = await controller.GetProductsByCategory("Electronics", 1, 10);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task ProductExists_ReturnsOk_WhenExists()
    {
        // Arrange
        var fakeService = A.Fake<IProductService>();
        A.CallTo(() => fakeService.ExistsAsync(A<Guid>.Ignored))
            .Returns(Task.FromResult(true));
        var controller = CreateController(fakeService);

        // Act
        var result = await controller.ProductExists(Guid.NewGuid());

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [Fact]
    public async Task ProductExists_ReturnsNotFound_WhenNotExists()
    {
        // Arrange
        var fakeService = A.Fake<IProductService>();
        A.CallTo(() => fakeService.ExistsAsync(A<Guid>.Ignored))
            .Returns(Task.FromResult(false));
        var controller = CreateController(fakeService);

        // Act
        var result = await controller.ProductExists(Guid.NewGuid());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}
