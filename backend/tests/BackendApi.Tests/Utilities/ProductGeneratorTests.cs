using System;
using System.Collections.Generic;
using BackendApi.DTOs;
using BackendApi.Models;
using BackendApi.Utilities;
using FluentAssertions;
using Xunit;

namespace BackendApi.Tests.Utilities
{
    public class ProductGeneratorTests
    {
        [Fact]
        public void GenerateProducts_ShouldReturnCorrectCount()
        {
            // Act
            var products = ProductGenerator.GenerateProducts(10);

            // Assert
            products.Should().NotBeNull();
            products.Should().HaveCount(10);
        }

        [Fact]
        public void GenerateProducts_ShouldGenerateUniqueSKUs()
        {
            var products = ProductGenerator.GenerateProducts(50);
            var skus = new HashSet<string>();
            foreach (var product in products)
            {
                skus.Add(product.SKU);
            }
            skus.Count.Should().Be(50);
        }

        [Fact]
        public void GenerateProducts_WithTemplate_ShouldApplyTemplateValues()
        {
            var template = new ProductDto
            {
                Name = "TestProduct",
                Category = "Electronics",
                Brand = "TechCorp",
                SKU = "TSTSKU",
                Price = 99.99m,
                StockQuantity = 100,
                ReleaseDate = new DateTime(2024, 1, 1),
                Description = "Template Description",
                AvailabilityStatus = "Available",
                CustomerRating = 4.5m,
                AvailableColors = "Red, Blue",
                AvailableSizes = "M, L"
            };
            var products = ProductGenerator.GenerateProducts(5, template);
            products.Should().OnlyContain(p => p.Category == "Electronics" && p.Brand == "TechCorp" && p.AvailabilityStatus == "Available");
            products.Should().OnlyContain(p => p.AvailableColors == "Red, Blue" && p.AvailableSizes == "M, L");
            products.Should().OnlyContain(p => p.Description.Contains("Template Description"));
        }

        [Fact]
        public void GenerateProducts_ShouldHandleZeroCount()
        {
            var products = ProductGenerator.GenerateProducts(0);
            products.Should().NotBeNull();
            products.Should().BeEmpty();
        }

        [Fact]
        public void GenerateProducts_ShouldHandleNullTemplate()
        {
            var products = ProductGenerator.GenerateProducts(3, null);
            products.Should().HaveCount(3);
        }

        [Fact]
        public void GenerateProducts_ShouldNotGenerateNegativeStockOrPrice()
        {
            var products = ProductGenerator.GenerateProducts(20);
            products.Should().OnlyContain(p => p.StockQuantity >= 0 && p.Price > 0);
        }

        [Fact]
        public void GenerateProducts_ShouldGenerateValidCustomerRatings()
        {
            var products = ProductGenerator.GenerateProducts(20);
            products.Should().OnlyContain(p => p.CustomerRating >= 1.0m && p.CustomerRating <= 5.0m);
        }
    }
}
