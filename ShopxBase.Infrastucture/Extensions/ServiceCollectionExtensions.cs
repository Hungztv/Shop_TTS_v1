using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ShopxBase.Infrastructure.Data;
using ShopxBase.Infrastructure.Data.Repositories;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Get connection string from appsettings.json
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

            // Register DbContext - PostgreSQL (Supabase)
            services.AddDbContext<ShopxBaseDbContext>(options =>
                options.UseNpgsql(connectionString,
                    postgresOptions => postgresOptions.CommandTimeout(30)));

            // Also register DbContext as abstract type for UnitOfWork injection
            services.AddScoped<DbContext>(sp => sp.GetRequiredService<ShopxBaseDbContext>());

            // Register Generic Repository<T> - for all basic CRUD operations
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Register Specialized Repositories - for entities with custom business queries
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICouponRepository, CouponRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Register Unit of Work - manages all repositories and transactions
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register External Services (TODO: implement these services)
            // services.AddScoped<IEmailService, EmailService>();
            // services.AddScoped<IPaymentService, PaymentService>();

            return services;
        }
    }
}
