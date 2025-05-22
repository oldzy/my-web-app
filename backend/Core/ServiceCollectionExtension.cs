using Core.UseCases.Abstractions;
using Core.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddTransient<IUserUseCases, UserUseCases>();
        services.AddTransient<IProductUseCases, ProductUseCases>();

        return services;
    }
}
