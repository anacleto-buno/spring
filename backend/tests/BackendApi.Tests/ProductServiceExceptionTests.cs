using Xunit;
using FluentAssertions;
using FakeItEasy;
using AutoMapper;
using BackendApi.Services;
using BackendApi.Services.Interfaces;
using BackendApi.Repositories.Interfaces;
using BackendApi.DTOs.Product;
using BackendApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductServiceExceptionTests
{
    [Fact]
    public async Task GetProductByIdAsync_ThrowsException_Propagates()
    {
        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        var fakeMapper = A.Fake<IMapper>();
        var fakeRepo = A.Fake<IProductRepository>();
        
        A.CallTo(() => fakeUnitOfWork.Products).Returns(fakeRepo);
        A.CallTo(() => fakeRepo.GetByIdAsync(A<Guid>.Ignored)).Throws(new Exception("DB error"));
        
        var service = new ProductService(fakeUnitOfWork, fakeMapper);
        
        Func<Task> act = async () => await service.GetProductByIdAsync(Guid.NewGuid());
        await act.Should().ThrowAsync<Exception>().WithMessage("DB error");
    }

    [Fact]
    public async Task CreateProductAsync_ThrowsException_Propagates()
    {
        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        var fakeMapper = A.Fake<IMapper>();
        var fakeRepo = A.Fake<IProductRepository>();
        
        A.CallTo(() => fakeUnitOfWork.Products).Returns(fakeRepo);
        A.CallTo(() => fakeRepo.AddAsync(A<Product>.Ignored)).Throws(new Exception("Create failed"));
        
        var service = new ProductService(fakeUnitOfWork, fakeMapper);
        var dto = new ProductCreateDto { Name = "Test", Price = 10.99m };
        
        Func<Task> act = async () => await service.CreateProductAsync(dto);
        await act.Should().ThrowAsync<Exception>().WithMessage("Create failed");
    }
}
