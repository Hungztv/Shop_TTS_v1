using ShopxBase.Application.Extensions;
using ShopxBase.Infrastructure.Extensions;
using ShopxBase.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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
// Workaround for connection pool issues with Supabase/PgBouncer
NpgsqlConnection.ClearAllPools();

var connectionString = $"Host={host};Database={database};Username={user};Password={password};Port={port};SslMode=Require;No Reset On Close=true;Pooling=false;Multiplexing=false;";

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

// Add connection string to configuration for Infrastructure layer
builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;

// Add Controllers
builder.Services.AddControllers();

// Add Swagger documentation with JWT support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ShopxBase API", Version = "v1", Description = "E-Commerce API with CQRS Pattern" });

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token."
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add Application Layer (MediatR, AutoMapper, FluentValidation)
builder.Services.AddApplication();

// Add Infrastructure Layer (DbContext, Repositories, Identity, JWT)
builder.Services.AddInfrastructure(builder.Configuration);

// Add JWT Authentication
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
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]!))
    };
});

builder.Services.AddAuthorization();

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

// Seed roles
using (var scope = app.Services.CreateScope())
{
    await SeedData.Initialize(scope.ServiceProvider);
}

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
