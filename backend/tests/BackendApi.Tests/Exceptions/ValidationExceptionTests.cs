using Xunit;
using BackendApi.Exceptions;

namespace BackendApi.Tests.Exceptions
{
    public class ValidationExceptionTests
    {
        [Fact]
        public void Constructor_SetsMessage()
        {
            var ex = new ValidationException("Validation failed");
            Assert.Equal("Validation failed", ex.Message);
        }
    }
}
