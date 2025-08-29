using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using BackendApi.Middleware;
using System.IO;

namespace BackendApi.Tests.Middleware
{
    public class ValidationMiddlewareTests
    {
        [Fact]
        public async Task Passes_Through_Request()
        {
            var context = new DefaultHttpContext();
            var logger = new LoggerFactory().CreateLogger<ValidationMiddleware>();
            var middleware = new ValidationMiddleware(async ctx => { ctx.Response.StatusCode = 200; });

            var responseStream = new MemoryStream();
            context.Response.Body = responseStream;

            await middleware.InvokeAsync(context);
            Assert.Equal(200, context.Response.StatusCode);
        }
    }
}
