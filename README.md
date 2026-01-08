# Shopping System - Clean Architecture .NET Project

Một dự án .NET hoàn chỉnh theo mô hình **Clean Architecture** với 4 tầng chính.

## 📁 Cấu Trúc Dự Án

```
Shop_TTS_V1/
├── ShopxBase.Domain/                  # Tầng Domain (Business Logic)
│   ├── Entities/                      # Các entity chính
│   │   ├── BaseEntity.cs              # Base class cho tất cả entities
│   │   ├── User.cs                    # User entity
│   │   ├── Product.cs                 # Product entity
│   │   └── Order.cs                   # Order & OrderItem entities
│   ├── Enums/                         # Các enum
│   │   ├── OrderStatus.cs             # Trạng thái đơn hàng
│   │   └── PaymentStatus.cs           # Trạng thái thanh toán
│   ├── Exceptions/                    # Custom exceptions
│   │   └── DomainException.cs         # Domain-specific exceptions
│   └── Interfaces/                    # Interfaces
│       ├── IRepository.cs             # Generic repository interface
│       └── IUnitOfWork.cs             # Unit of Work pattern
│
├── ShopxBase.Application/             # Tầng Application (Use Cases)
│   ├── DTOs/                          # Data Transfer Objects
│   │   ├── ProductDto.cs              # Product DTOs
│   │   └── OrderDto.cs                # Order DTOs
│   ├── Interfaces/                    # Service interfaces
│   │   ├── IProductService.cs         # Product service interface
│   │   └── IOrderService.cs           # Order service interface
│   ├── Services/                      # Service implementations
│   │   ├── ProductService.cs          # Product service
│   │   └── OrderService.cs            # Order service
│   └── MediatR/                       # MediatR pattern (tùy chọn)
│       ├── Commands/                  # Commands
│       ├── Queries/                   # Queries
│       └── Handlers/                  # Handlers
│
├── ShopxBase.Infrastructure/          # Tầng Infrastructure (Data Access)
│   ├── Data/
│   │   ├── DbContext/                 # Entity Framework DbContext
│   │   │   └── ShoppingDbContext.cs   # Shopping DB context
│   │   ├── Repositories/              # Repository implementations
│   │   │   ├── Repository.cs          # Generic repository
│   │   │   └── ProductRepository.cs   # Product repository
│   │   └── UnitOfWork.cs              # Unit of Work implementation
│   ├── Services/                      # External services
│   │   ├── EmailService.cs            # Email service
│   │   └── PaymentService.cs          # Payment service
│   └── Persistence/                   # Migration & seed data
│
├── ShopxBase.Api/                     # Tầng Presentation (API)
│   ├── Controllers/                   # API Controllers
│   │   ├── ProductsController.cs      # Products API endpoints
│   │   └── OrdersController.cs        # Orders API endpoints
│   ├── Models/                        # Request/Response models
│   │   └── ApiModels.cs               # API response wrappers
│   ├── Program.cs                     # Application startup & DI configuration
│   ├── appsettings.json               # Configuration
│   └── appsettings.Development.json   # Development configuration
│
├── Shopping.slnx                      # Solution file
├── global.json                        # SDK configuration
└── README.md                          # This file
```

## 🏗️ Kiến Trúc Clean Architecture

### Domain Layer (ShopxBase.Domain)

- **Mục đích**: Chứa business logic, rules, entities, exceptions
- **Không phụ thuộc vào**: Application, Infrastructure, API
- **Chứa**: Entities, Enums, Interfaces (IRepository, IUnitOfWork), Exceptions

### Application Layer (ShopxBase.Application)

- **Mục đích**: Chứa use cases, DTOs, service interfaces
- **Phụ thuộc vào**: Domain Layer
- **Chứa**: DTOs, Service Interfaces, Service Implementations, MediatR handlers

### Infrastructure Layer (ShopxBase.Infrastructure)

- **Mục đích**: Implement data access, external services
- **Phụ thuộc vào**: Domain, Application
- **Chứa**: DbContext, Repositories, Unit of Work, External Services

### Presentation Layer (ShopxBase.Api)

- **Mục đích**: API endpoints, HTTP handling
- **Phụ thuộc vào**: Application, Infrastructure
- **Chứa**: Controllers, Models, Program.cs (DI configuration)

## 🚀 Getting Started

### Prerequisites

- .NET 10.0 SDK
- SQL Server (LocalDB or Express)

### Setup

1. **Restore NuGet packages**:

```bash
cd d:\Shop_TTS_V1
dotnet restore
```

2. **Build solution**:

```bash
dotnet build Shopping.slnx
```

3. **Create database**:

```bash
cd ShopxBase.Api/ShopxBase.Api
dotnet ef database update
```

4. **Run application**:

```bash
dotnet run
```

API sẽ chạy tại: `https://localhost:5001` (hoặc port khác)

## 📚 API Endpoints

### Products

- `GET /api/products` - Lấy tất cả sản phẩm
- `GET /api/products/{id}` - Lấy sản phẩm theo ID
- `POST /api/products` - Tạo sản phẩm mới
- `PUT /api/products/{id}` - Cập nhật sản phẩm
- `DELETE /api/products/{id}` - Xóa sản phẩm
- `GET /api/products/category/{category}` - Lấy sản phẩm theo category

### Orders

- `GET /api/orders` - Lấy tất cả đơn hàng
- `GET /api/orders/{id}` - Lấy đơn hàng theo ID
- `GET /api/orders/user/{userId}` - Lấy đơn hàng của user
- `POST /api/orders` - Tạo đơn hàng mới
- `PUT /api/orders/{id}/status` - Cập nhật trạng thái đơn hàng
- `DELETE /api/orders/{id}/cancel` - Hủy đơn hàng
- `GET /api/orders/user/{userId}/count` - Đếm đơn hàng của user

## 🔧 Configuration

### Connection String

Edit `appsettings.json` để cấu hình connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ShoppingDB;Trusted_Connection=true;"
  }
}
```

### Dependency Injection

Tất cả dependencies được cấu hình trong `Program.cs`:

```csharp
// Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Infrastructure
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
```

## 📦 NuGet Packages

- **Entity Framework Core 9.0.0** - ORM
- **Swashbuckle.AspNetCore 6.4.0** - Swagger/OpenAPI
- **Microsoft.AspNetCore.App** - ASP.NET Core

## 🔐 Security Considerations

- Thêm authentication/authorization
- Validate input data
- Implement error handling
- Add CORS configuration
- Use HTTPS in production

## 📝 Next Steps

1. **Add Authentication**: Implement JWT authentication
2. **Add Validation**: Use FluentValidation
3. **Add Logging**: Configure Serilog
4. **Add Unit Tests**: Create xUnit tests
5. **Add Migrations**: Setup EF Core migrations
6. **Implement Caching**: Add Redis caching
7. **Add API Documentation**: Enhance Swagger docs

## 📄 License

Dự án này là một mẫu học tập cho Clean Architecture trong .NET.

## 👨‍💻 Author

Tạo bằng terminal với `dotnet new` command và .NET 10.0

---

**Happy Coding!** 🎉
