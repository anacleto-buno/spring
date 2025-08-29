using Xunit;
using FluentValidation.TestHelper;
using BackendApi.DTOs.Product;
using BackendApi.Validation;

namespace BackendApi.Tests.Validation
{
    public class ProductFilterValidatorTests
    {
        private readonly ProductFilterValidator _validator = new ProductFilterValidator();

        [Fact]
        public void Should_Have_Error_When_Page_Is_Less_Than_One()
        {
            var model = new ProductFilterDto { Page = 0, PageSize = 10 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Page);
        }


        [Fact]
        public void Should_Clamp_PageSize_To_Max_When_Above_100()
        {
            var model = new ProductFilterDto { Page = 1, PageSize = 101 };
            Assert.Equal(100, model.PageSize);
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.PageSize);
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Model()
        {
            var model = new ProductFilterDto { Page = 1, PageSize = 10 };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
