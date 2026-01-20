using ShopxBase.Application.Extensions;
using ShopxBase.Infrastructure.Extensions;
using ShopxBase.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using DotNetEnv;

// Load environment variables from .env file
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Build connection string from environment variables
var host = Environment.GetEnvironmentVariable("SUPABASE_HOST") ?? "localhost";
var database = Environment.GetEnvironmentVariable("SUPABASE_DATABASE") ?? "postgres";
var user = Environment.GetEnvironmentVariable("SUPABASE_USER") ?? "postgres";
var password = Environment.GetEnvironmentVariable("SUPABASE_PASSWORD") ?? "postgres";
var connectionString = $"Host={host};Database={database};Username={user};Password={password};SSL Mode=Require;Trust Server Certificate=true";

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ShopxBaseDbContext>(options =>
    options.UseNpgsql(connectionString)
);

// Add Application Layer (MediatR, AutoMapper, FluentValidation)
builder.Services.AddApplication();

// Add Infrastructure Layer (Repositories, UnitOfWork, Services)
builder.Services.AddInfrastructure(builder.Configuration);

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
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.MapControllers();

app.Run();
