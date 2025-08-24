
using BackendApi.Models;
using BackendApi.Repositories;
using BackendApi.Repositories.Interfaces;
using BackendApi.Services;
using BackendApi.Services.Interfaces;
using BackendApi.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository registration
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Service registration
builder.Services.AddScoped<IProductService, ProductService>();

// Add controllers with API behavior options
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = false; // Keep model validation
    });

// API Explorer and Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Product API",
        Version = "v1",
        Description = "A comprehensive Product Management API built with .NET 9",
    });

    // Include XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Add CORS if needed
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:4200") // Add your frontend URLs
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Add global exception handling middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

// Database initialization with retry logic
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    var retryCount = 0;
    const int maxRetries = 10;
    
    while (retryCount < maxRetries)
    {
        try
        {
            // Ensure database is created and apply any pending migrations
            await dbContext.Database.EnsureCreatedAsync();
            logger.LogInformation("Database and tables are ready.");
            break;
        }
        catch (Exception ex)
        {
            retryCount++;
            logger.LogWarning("Attempt {RetryCount}/{MaxRetries} failed to connect to database: {Error}", 
                retryCount, maxRetries, ex.Message);
            
            if (retryCount >= maxRetries)
            {
                logger.LogError(ex, "Failed to connect to database after {MaxRetries} attempts. Continuing without database initialization.", maxRetries);
                break;
            }
            
            await Task.Delay(2000); // Wait 2 seconds before retry
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API V1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at the app's root
    });
}

// Security headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    await next();
});

app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowSpecificOrigins");

// Add authentication/authorization middleware if needed
// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();

// Health check endpoint
app.MapGet("/health", async (AppDbContext dbContext) =>
{
    try
    {
        await dbContext.Database.CanConnectAsync();
        return Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Unhealthy: {ex.Message}");
    }
});

app.Run();
