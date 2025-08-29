using Xunit;
using BackendApi.Validation;

namespace BackendApi.Tests.Validation
{
    public class ValidationHelperTests
    {
        private class TestModel
        {
            public string Name { get; set; } = string.Empty;
        }

        [Fact]
        public void ValidateObject_ReturnsError_WhenInvalid()
        {
            var model = new TestModel { Name = "" };
            var errors = ValidationHelper.ValidateObject(model);
            Assert.NotEmpty(errors);
        }

        [Fact]
        public void ValidateObject_ReturnsEmpty_WhenValid()
        {
            var model = new TestModel { Name = "Valid" };
            var errors = ValidationHelper.ValidateObject(model);
            Assert.Empty(errors);
        }
    }
}
