# .NET 9 Web API with Optimized Architecture

This project is a production-ready C# .NET 9 Web API following enterprise architecture patterns including Repository Pattern, Service Layer, DTOs, Dependency Injection, and comprehensive error handling. It uses Entity Framework Core, LINQ, and is fully containerized with Docker and PostgreSQL.

## 🏗️ Architecture Features

### **Clean Architecture Patterns**
- **Repository Pattern** - Data access abstraction layer
- **Service Layer** - Business logic separation  
- **DTOs (Data Transfer Objects)** - API contract models
- **Dependency Injection** - Loose coupling and testability
- **Global Exception Handling** - Centralized error management
- **Input Validation** - Data integrity and security
- **Structured Logging** - Comprehensive application monitoring

### **API Features**
- **Controller-based architecture** with proper HTTP status codes
- **Swagger/OpenAPI documentation** with XML comments
- **Model validation** with custom attributes
- **CORS support** for cross-origin requests
- **Security headers** (XSS, Content-Type, Frame protection)
- **Health check endpoint** for monitoring
- **Structured error responses**

## 🔧 Technology Stack

- **.NET 9** - Latest .NET framework
- **Entity Framework Core 9.0** - ORM with PostgreSQL
- **PostgreSQL** - Production database
- **Docker & Docker Compose** - Containerization
- **Swagger/OpenAPI** - API documentation
- **LINQ** - Query operations
- **Dependency Injection** - Built-in DI container

## 📊 Product Model Structure

```csharp
- Id: Guid (Primary Key, Auto-generated)
- SKU: string(100) - Product Stock Keeping Unit
- Description: text - Detailed product description
- UOM: string(100) - Unit of Measure
- UnitPrice: decimal(15,2) - Product unit price
```


# Spring Backend API

This is the backend API for the Spring project, built with C# .NET 9, Entity Framework Core, LINQ, Docker, and PostgreSQL.

## Features

- C# .NET 9, EF Core, LINQ
- Dockerized with PostgreSQL
- Product CRUD operations
- Bulk product generation
- Comprehensive filtering/search API

## Getting Started

1. Clone the repository
2. Build the project: `dotnet build`
3. Run with Docker Compose: `docker-compose up -d`
4. Access API at `http://localhost:5000`

## API Endpoints

### Products
- `GET /api/product` - Get all products
- `GET /api/product/{id}` - Get product by ID
- `GET /api/product/by-sku/{sku}` - Get product by SKU
- `POST /api/product` - Create product
- `PUT /api/product/{id}` - Update product
- `DELETE /api/product/{id}` - Delete product
- `POST /api/product/generate/{count}` - Bulk generate products

### Filtering/Search
- `GET /api/product/search?searchTerm={term}` - Search products by any property (name, description, category, brand, SKU, availability status, colors, sizes, etc)
  - Case-insensitive, partial match across all major product fields
  - Returns 400 Bad Request for empty/null search terms

#### Example Searches
```http
GET /api/product/search?searchTerm=phone
GET /api/product/search?searchTerm=Variant 407
GET /api/product/search?searchTerm=SKU
GET /api/product/search?searchTerm=Generated
```

## Configuration

See `appsettings.json` for database connection and other settings.

## Development

- Optimized architecture and error handling
- Global exception middleware
- DTOs for API responses
- Logging for all major operations

## Testing

Use the included `BackendApi.http` file for sample requests and API testing.

## License

MIT
├── Controllers/          # API Controllers
│   └── ProductController.cs
├── DTOs/                # Data Transfer Objects
│   └── ProductDto.cs
├── Models/              # Domain Models & DbContext
│   ├── Product.cs       # Product entity
│   └── AppDbContext.cs  # EF Core DbContext
├── Repositories/        # Data Access Layer
│   ├── Interfaces/
│   │   └── IProductRepository.cs
│   └── ProductRepository.cs
├── Services/            # Business Logic Layer
│   ├── Interfaces/
│   │   └── IProductService.cs
│   └── ProductService.cs
├── Middleware/          # Custom Middleware
│   └── GlobalExceptionMiddleware.cs
├── Validation/          # Custom Validation
│   └── ValidationHelper.cs
└── Program.cs          # Application Startup
```

## 🔧 Configuration

### **Connection Strings**
Update `appsettings.json` for local development:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=appdb;Username=postgres;Password=postgres"
  }
}
```

### **Docker Environment**
Connection string for Docker is configured in `docker-compose.yml`:
- **Database Host**: `db` (container name)
- **Database Port**: `5432`
- **API Port**: `5000`

## 🔍 API Documentation

Once running, access comprehensive API documentation:
- **Swagger UI**: http://localhost:5000
- **OpenAPI JSON**: http://localhost:5000/swagger/v1/swagger.json

## 🏥 Health Monitoring

- **Health Check**: http://localhost:5000/health
- **Returns**: Database connectivity status and timestamp

## 🔒 Security Features

- **Global Exception Handling** - No sensitive data exposure
- **Input Validation** - Prevents malicious data
- **Security Headers** - XSS, CSRF, Clickjacking protection
- **CORS Policy** - Configurable cross-origin access
- **Model Validation** - Data integrity enforcement

## 🧪 Development Features

- **XML Documentation** generation for Swagger
- **Structured Logging** with different log levels
- **Environment-specific configurations**
- **Hot reload support** in development
- **Database auto-creation** on startup

## 📝 Sample Usage

### **Create a Product**
```bash
curl -X POST "http://localhost:5000/api/products" \
  -H "Content-Type: application/json" \
  -d '{
    "sku": "PROD-001",
    "description": "Sample Product",
    "uom": "Each",
    "unitPrice": 29.99
  }'
```

### **Get All Products**
```bash
curl -X GET "http://localhost:5000/api/products"
```

This architecture provides a solid foundation for enterprise-level applications with maintainability, testability, and scalability in mind! 🚀
