using ShopxBase.Application.Extensions;
using ShopxBase.Infrastructure.Extensions;
using ShopxBase.Infrastructure.Data;
using ShopxBase.Infrastructure.Data.Repositories;
using ShopxBase.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using Npgsql;
using ShopxBase.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using ShopxBase.Application.Interfaces;
using ShopxBase.Infrastructure.Services;
using ShopxBase.Application.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

// 1. Load environment variables from .env file
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

// 2. Build connection string from environment variables (.env file)
var host = Environment.GetEnvironmentVariable("SUPABASE_HOST")
    ?? throw new InvalidOperationException("SUPABASE_HOST is not set in .env file");
var port = Environment.GetEnvironmentVariable("SUPABASE_PORT") ?? "6543";
var database = Environment.GetEnvironmentVariable("SUPABASE_DATABASE")
    ?? throw new InvalidOperationException("SUPABASE_DATABASE is not set in .env file");
var user = Environment.GetEnvironmentVariable("SUPABASE_USER")
    ?? throw new InvalidOperationException("SUPABASE_USER is not set in .env file");
var password = Environment.GetEnvironmentVariable("SUPABASE_PASSWORD")
    ?? throw new InvalidOperationException("SUPABASE_PASSWORD is not set in .env file");

Console.WriteLine($"📡 Connecting to: {host}:{port}/{database}");

// Configure connection pooling
NpgsqlConnection.ClearAllPools();

var connectionString = $"Host={host};Database={database};Username={user};Password={password};Port={port};SslMode=Require;Connection Idle Lifetime=300;Pooling=true;Maximum Pool Size=20;Minimum Pool Size=5;";

builder.Services.AddDbContext<ShopxBaseDbContext>(options =>
    options.UseNpgsql(connectionString)
);

// 3. Configure Identity
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;

    // User settings
    options.User.RequireUniqueEmail = true;

    // Lockout settings (optional)
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
})
.AddEntityFrameworkStores<ShopxBaseDbContext>()
.AddDefaultTokenProviders();

// 4. Configure Supabase Settings
var supabaseUrl = Environment.GetEnvironmentVariable("SUPABASE_URL")
    ?? throw new InvalidOperationException("SUPABASE_URL is not set in .env file");
var supabaseAnonKey = Environment.GetEnvironmentVariable("SUPABASE_ANON_KEY")
    ?? throw new InvalidOperationException("SUPABASE_ANON_KEY is not set in .env file");
var supabaseJwtSecret = Environment.GetEnvironmentVariable("SUPABASE_JWT_SECRET")
    ?? throw new InvalidOperationException("SUPABASE_JWT_SECRET is not set in .env file");

builder.Services.Configure<SupabaseSettings>(options =>
{
    options.Url = supabaseUrl;
    options.AnonKey = supabaseAnonKey;
    options.JwtSecret = supabaseJwtSecret;
});

// 5. Configure JWT Settings (legacy - still used for custom tokens)
var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettingsSection);

var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
var secretKey = jwtSettings?.SecretKey
    ?? Environment.GetEnvironmentVariable("JWT_SECRET") // Fallback lấy từ ENV nếu config null
    ?? throw new InvalidOperationException("JWT SecretKey is not configured");

// 6. Configure JWT Authentication with Supabase JWT support
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        // Support both Supabase issuer and custom issuer
        ValidIssuers = new[]
        {
            $"{supabaseUrl}/auth/v1",  // Supabase Auth issuer
            jwtSettings?.Issuer ?? "ShopxBaseAPI"  // Custom issuer
        },
        ValidAudiences = new[]
        {
            "authenticated",  // Supabase audience
            jwtSettings?.Audience ?? "ShopxBaseClient"  // Custom audience
        },
        // Use Supabase JWT secret for verification
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(supabaseJwtSecret)),
        ClockSkew = TimeSpan.Zero,
        // Map Supabase claims to standard claims
        RoleClaimType = "role",
        NameClaimType = "email"
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"❌ Authentication failed: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var userId = context.Principal?.FindFirst("sub")?.Value;
            var email = context.Principal?.FindFirst("email")?.Value;
            var role = context.Principal?.FindFirst("role")?.Value;
            Console.WriteLine($"✅ Token validated - User: {userId}, Email: {email}, Role: {role}");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// 7. Add Controllers
builder.Services.AddControllers();

// 6. Add Swagger documentation (ĐÃ SỬA LỖI Ở ĐÂY)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ShopxBase API", Version = "v1", Description = "E-Commerce API with CQRS Pattern" });

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Authorization: Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer" // <-- Đã thêm dòng này (Quan trọng)
                }
            },
            Array.Empty<string>() // <-- Đã thêm mảng rỗng (Quan trọng)
        }
    });
});

// 9. Add Application Layer (MediatR, AutoMapper, FluentValidation)
builder.Services.AddApplication();

// 10. Add Infrastructure Layer
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICouponRepository, CouponRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<DbContext>(sp => sp.GetRequiredService<ShopxBaseDbContext>());
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add Auth Services
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// Add Supabase Auth Service
builder.Services.AddHttpClient<ISupabaseAuthService, SupabaseAuthService>();

// 11. Add CORS
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

// 10. Configure the HTTP request pipeline
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// 11. Seed roles and admin user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        DbInitializer.SeedRolesAndAdminAsync(services).Wait();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error seeding database: {ex.Message}");
    }
}

app.Run();