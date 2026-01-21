using ShopxBase.Application.Extensions;
using ShopxBase.Infrastructure.Extensions;
using ShopxBase.Infrastructure.Data;
using ShopxBase.Infrastructure.Data.Repositories;
using ShopxBase.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using Npgsql;

// Load environment variables from .env file
// Find .env from solution root (2 levels up from ShopxBase.Api)
var solutionRoot = Directory.GetCurrentDirectory();
while (!File.Exists(Path.Combine(solutionRoot, ".env")) && solutionRoot != Path.GetPathRoot(solutionRoot))
{
    solutionRoot = Path.GetDirectoryName(solutionRoot) ?? solutionRoot;
}

var envPath = Path.Combine(solutionRoot, ".env");
if (File.Exists(envPath))
{
    Env.Load(envPath);
    Console.WriteLine($"✅ Loaded .env from: {envPath}");
}
else
{
    Console.WriteLine($"❌ .env file not found at: {envPath}");
}

var builder = WebApplication.CreateBuilder(args);

// Build connection string from environment variables (.env file)
var host = Environment.GetEnvironmentVariable("SUPABASE_HOST")
    ?? throw new InvalidOperationException("SUPABASE_HOST is not set in .env file");
var port = Environment.GetEnvironmentVariable("SUPABASE_PORT") ?? "6543";
var database = Environment.GetEnvironmentVariable("SUPABASE_DATABASE")
    ?? throw new InvalidOperationException("SUPABASE_DATABASE is not set in .env file");
var user = Environment.GetEnvironmentVariable("SUPABASE_USER")
    ?? throw new InvalidOperationException("SUPABASE_USER is not set in .env file");
var password = Environment.GetEnvironmentVariable("SUPABASE_PASSWORD")
    ?? throw new InvalidOperationException("SUPABASE_PASSWORD is not set in .env file");

Console.WriteLine($" Connecting to: {host}:{port}/{database}");
// Workaround for connection pool issues
NpgsqlConnection.ClearAllPools();

var connectionString = $"Host={host};Database={database};Username={user};Password={password};Port={port};SslMode=Require;No Reset On Close=true;";

// Load JWT Settings from .env and add to Configuration
var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET")
    ?? throw new InvalidOperationException("JWT_SECRET is not set in .env file");
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "ShopxBaseAPI";
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "ShopxBaseClient";
var jwtExpiryMinutes = Environment.GetEnvironmentVariable("JWT_EXPIRY_MINUTES") ?? "60";

// Add JWT settings to Configuration
builder.Configuration["JwtSettings:Secret"] = jwtSecret;
builder.Configuration["JwtSettings:Issuer"] = jwtIssuer;
builder.Configuration["JwtSettings:Audience"] = jwtAudience;
builder.Configuration["JwtSettings:ExpiryMinutes"] = jwtExpiryMinutes;

builder.Services.AddDbContext<ShopxBaseDbContext>(options =>
    options.UseNpgsql(connectionString)
);

// Add Controllers
builder.Services.AddControllers();

// Add Swagger documentation
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ShopxBase API", Version = "v1", Description = "E-Commerce API with CQRS Pattern" });
});

// Add Application Layer (MediatR, AutoMapper, FluentValidation)
builder.Services.AddApplication();

// Add Infrastructure Layer - manually add repositories since DbContext is already configured
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICouponRepository, CouponRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<DbContext>(sp => sp.GetRequiredService<ShopxBaseDbContext>());
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add CORS if needed
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Add logging
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShopxBase API v1");
        c.RoutePrefix = string.Empty; // Swagger at root
    });
}

// app.UseHttpsRedirection(); // Disabled for development (HTTP only)
app.UseCors("AllowAll");
app.MapControllers();

app.Run();
