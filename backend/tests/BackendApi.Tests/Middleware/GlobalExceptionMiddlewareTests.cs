using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using BackendApi.Middleware;
using BackendApi.Exceptions;
using System.IO;

namespace BackendApi.Tests.Middleware
{
    public class GlobalExceptionMiddlewareTests
    {
        [Fact]
        public async Task Handles_BusinessException_Returns_BadRequest()
        {
            var context = new DefaultHttpContext();
            var logger = new LoggerFactory().CreateLogger<GlobalExceptionMiddleware>();
            var middleware = new GlobalExceptionMiddleware(async ctx => throw new BusinessException("Business error"), logger);

            var responseStream = new MemoryStream();
            context.Response.Body = responseStream;

            await middleware.InvokeAsync(context);
            Assert.Equal(400, context.Response.StatusCode);
        }

        [Fact]
        public async Task Handles_ValidationException_Returns_BadRequest()
        {
            var context = new DefaultHttpContext();
            var logger = new LoggerFactory().CreateLogger<GlobalExceptionMiddleware>();
            var middleware = new GlobalExceptionMiddleware(async ctx => throw new ValidationException("Validation error"), logger);

            var responseStream = new MemoryStream();
            context.Response.Body = responseStream;

            await middleware.InvokeAsync(context);
            Assert.Equal(400, context.Response.StatusCode);
        }

        [Fact]
        public async Task Handles_NotFoundException_Returns_NotFound()
        {
            var context = new DefaultHttpContext();
            var logger = new LoggerFactory().CreateLogger<GlobalExceptionMiddleware>();
            var middleware = new GlobalExceptionMiddleware(async ctx => throw new NotFoundException("Not found"), logger);

            var responseStream = new MemoryStream();
            context.Response.Body = responseStream;

            await middleware.InvokeAsync(context);
            Assert.Equal(404, context.Response.StatusCode);
        }
    }
}
