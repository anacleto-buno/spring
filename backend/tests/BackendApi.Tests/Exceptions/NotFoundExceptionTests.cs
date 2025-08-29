using Xunit;
using BackendApi.Exceptions;

namespace BackendApi.Tests.Exceptions
{
    public class NotFoundExceptionTests
    {
        [Fact]
        public void Constructor_SetsMessage()
        {
            var ex = new NotFoundException("Not found");
            Assert.Equal("Not found", ex.Message);
        }
    }
}
