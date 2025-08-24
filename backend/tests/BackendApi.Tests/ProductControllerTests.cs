using Xunit;
using FluentAssertions;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BackendApi.Controllers;
using BackendApi.Services.Interfaces;
using BackendApi.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductControllerTests
{
    private ProductController CreateController(IProductService? service = null)
    {
        var fakeService = service ?? A.Fake<IProductService>();
        var fakeLogger = A.Fake<ILogger<ProductController>>();
        return new ProductController(fakeService, fakeLogger);
    }

    [Fact]
    public async Task GetProducts_ReturnsOkWithProducts()
    {
        var fakeService = A.Fake<IProductService>();
        A.CallTo(() => fakeService.GetAllProductsAsync()).Returns(Task.FromResult<IEnumerable<ProductDto>>(new[] { new ProductDto { Name = "Test" } }));
        var controller = CreateController(fakeService);
        var result = await controller.GetProducts();
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetProduct_ReturnsNotFound_WhenMissing()
    {
        var fakeService = A.Fake<IProductService>();
        A.CallTo(() => fakeService.GetProductByIdAsync(A<Guid>.Ignored)).Returns(Task.FromResult<ProductDto?>(null));
        var controller = CreateController(fakeService);
        var result = await controller.GetProduct(Guid.NewGuid());
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task SearchProducts_ReturnsBadRequest_WhenTermIsEmpty()
    {
        var controller = CreateController();
        var result = await controller.SearchProducts("");
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task SearchProducts_ReturnsOk_WhenTermIsValid()
    {
        var fakeService = A.Fake<IProductService>();
        A.CallTo(() => fakeService.SearchProductsAsync("apple")).Returns(Task.FromResult<IEnumerable<ProductDto>>(new[] { new ProductDto { Name = "Apple" } }));
        var controller = CreateController(fakeService);
        var result = await controller.SearchProducts("apple");
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task CreateProduct_ReturnsBadRequest_WhenModelInvalid()
    {
        var controller = CreateController();
        controller.ModelState.AddModelError("Name", "Required");
        var result = await controller.CreateProduct(new CreateProductDto());
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task CreateProduct_ReturnsCreated_WhenValid()
    {
        var fakeService = A.Fake<IProductService>();
        var dto = new CreateProductDto { Name = "Test" };
        var created = new ProductDto { Id = Guid.NewGuid(), Name = "Test" };
        A.CallTo(() => fakeService.CreateProductAsync(dto)).Returns(Task.FromResult(created));
        var controller = CreateController(fakeService);
        var result = await controller.CreateProduct(dto);
        result.Result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    public async Task UpdateProduct_ReturnsNotFound_WhenMissing()
    {
        var fakeService = A.Fake<IProductService>();
        A.CallTo(() => fakeService.UpdateProductAsync(A<Guid>.Ignored, A<UpdateProductDto>.Ignored)).Returns(Task.FromResult<ProductDto?>(null));
        var controller = CreateController(fakeService);
        var result = await controller.UpdateProduct(Guid.NewGuid(), new UpdateProductDto());
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task DeleteProduct_ReturnsNotFound_WhenMissing()
    {
        var fakeService = A.Fake<IProductService>();
        A.CallTo(() => fakeService.DeleteProductAsync(A<Guid>.Ignored)).Returns(Task.FromResult(false));
        var controller = CreateController(fakeService);
        var result = await controller.DeleteProduct(Guid.NewGuid());
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task DeleteProduct_ReturnsNoContent_WhenDeleted()
    {
        var fakeService = A.Fake<IProductService>();
        A.CallTo(() => fakeService.DeleteProductAsync(A<Guid>.Ignored)).Returns(Task.FromResult(true));
        var controller = CreateController(fakeService);
        var result = await controller.DeleteProduct(Guid.NewGuid());
        result.Should().BeOfType<NoContentResult>();
    }
}
