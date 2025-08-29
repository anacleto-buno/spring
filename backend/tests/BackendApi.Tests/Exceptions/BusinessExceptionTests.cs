using Xunit;
using BackendApi.Exceptions;

namespace BackendApi.Tests.Exceptions
{
    public class BusinessExceptionTests
    {
        [Fact]
        public void Constructor_SetsMessage()
        {
            var ex = new BusinessException("Test message");
            Assert.Equal("Test message", ex.Message);
        }
    }
}
