using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using MediatR;
using FluentValidation;

namespace ShopxBase.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        // Register AutoMapper - Scan all mapping profiles in this assembly
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(typeof(ServiceCollectionExtensions).Assembly);
        });

        // Register MediatR - Scan all commands, queries, handlers
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
        });

        // Register FluentValidation - Scan all validators
        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);

        return services;
    }
}