using Xunit;
using FluentAssertions;
using FakeItEasy;
using BackendApi.Services;
using BackendApi.Services.Interfaces;
using BackendApi.Repositories.Interfaces;
using BackendApi.DTOs;
using BackendApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductServiceExceptionTests
{
    [Fact]
    public async Task GetProductByIdAsync_ThrowsException_Propagates()
    {
        var fakeRepo = A.Fake<IProductRepository>();
        A.CallTo(() => fakeRepo.GetByIdAsync(A<Guid>.Ignored)).Throws(new Exception("DB error"));
        var service = new ProductService(fakeRepo);
        Func<Task> act = async () => await service.GetProductByIdAsync(Guid.NewGuid());
        await act.Should().ThrowAsync<Exception>().WithMessage("DB error");
    }

    [Fact]
    public async Task CreateProductAsync_ThrowsException_Propagates()
    {
        var fakeRepo = A.Fake<IProductRepository>();
        A.CallTo(() => fakeRepo.CreateAsync(A<Product>.Ignored)).Throws(new Exception("Create failed"));
        var service = new ProductService(fakeRepo);
        var dto = new CreateProductDto { Name = "Test" };
        Func<Task> act = async () => await service.CreateProductAsync(dto);
        await act.Should().ThrowAsync<Exception>().WithMessage("Create failed");
    }
}
