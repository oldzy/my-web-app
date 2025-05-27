using System;
using Core.IGateways;
using Infrastructure.Gateways;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient<ICartRepository, CartRepository>();
        services.AddTransient<IUserGateway, UserGateway>();
        services.AddTransient<IProductGateway, ProductGateway>();
        services.AddTransient<ICartGateway, CartGateway>();

        return services;
    }
}

