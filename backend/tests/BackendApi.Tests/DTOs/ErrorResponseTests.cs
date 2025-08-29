using Xunit;
using BackendApi.DTOs.Common;

namespace BackendApi.Tests.DTOs
{
    public class ErrorResponseTests
    {
        [Fact]
        public void Can_Set_Properties()
        {
            var error = new ErrorResponse
            {
                Message = "Error",
                Code = "ERR",
                CorrelationId = "123",
                Details = new { Info = "Details" }
            };
            Assert.Equal("Error", error.Message);
            Assert.Equal("ERR", error.Code);
            Assert.Equal("123", error.CorrelationId);
            Assert.NotNull(error.Details);
        }
    }
}
