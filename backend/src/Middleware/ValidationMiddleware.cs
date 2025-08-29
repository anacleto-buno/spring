using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using FluentValidation;
using System.Text.Json;
using System.Linq;
using System.Collections.Generic;

namespace BackendApi.Middleware
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Validation logic will be handled by FluentValidation filters
            await _next(context);
        }
    }
}
