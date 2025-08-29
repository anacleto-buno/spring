using Xunit;
using FluentValidation.TestHelper;
using BackendApi.DTOs.Product;
using BackendApi.Validation;

namespace BackendApi.Tests.Validation
{
    public class ProductUpdateValidatorTests
    {
        private readonly ProductUpdateValidator _validator = new ProductUpdateValidator();

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var model = new ProductUpdateDto { Name = "", Price = 10 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Price_Is_Zero()
        {
            var model = new ProductUpdateDto { Name = "Test", Price = 0 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Price);
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Model()
        {
            var model = new ProductUpdateDto { Name = "Test", Price = 10 };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
