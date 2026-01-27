# Shop TTS E-Commerce Platform

Monorepo structure for a full-stack e-commerce application built with **Clean Architecture**, **CQRS**, and **MediatR**.

## 📁 Project Structure

```
Shop_TTS_V1/
├── backend/                           # ASP.NET Core 10 API (Clean Architecture)
│   ├── ShopxBase.Domain/              # Domain Layer (Business Logic)
│   ├── ShopxBase.Application/         # Application Layer (Use Cases, CQRS)
│   ├── ShopxBase.Api/                 # API Layer (Controllers, Endpoints)
│   ├── ShopxBase.Infrastucture/       # Infrastructure Layer (Data Access, EF)
│   ├── Database/                      # Database scripts
│   ├── ShopxBase.slnx                 # Solution file
│   ├── global.json
│   └── .env                           # Environment variables (secrets, DB config)
│
├── frontend/                          # Next.js Frontend (Placeholder)
│
└── README.md                          # This file
```

## 📋 Backend Project Structure

For detailed backend architecture and features, see `backend/` directory.

**Layers:**

- **Domain** - Entities, Enums, Exceptions, Interfaces
- **Application** - DTOs, CQRS Commands/Queries, Handlers, Validators, Mappings
- **Infrastructure** - Data Access, EF Core DbContext, Repositories, Migrations
- **API** - Controllers, Middleware, Dependency Injection
  │
  ├── ShopxBase.Api/ # Tầng Presentation (API)
  │ ├── Controllers/ # API Controllers
  │ │ ├── ProductsController.cs # Products API endpoints
  │ │ └── OrdersController.cs # Orders API endpoints
  │ ├── Models/ # Request/Response models
  │ │ └── ApiModels.cs # API response wrappers
  │ ├── Program.cs # Application startup & DI configuration
  │ ├── appsettings.json # Configuration
  │ └── appsettings.Development.json # Development configuration
  │
  ├── Shopping.slnx # Solution file
  ├── global.json # SDK configuration
  └── README.md # This file

````

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
````

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
