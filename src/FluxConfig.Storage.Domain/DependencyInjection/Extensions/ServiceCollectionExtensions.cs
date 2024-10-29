using FluxConfig.Storage.Domain.Services;
using FluxConfig.Storage.Domain.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FluxConfig.Storage.Domain.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IStoragePublicService, StoragePublicService>();
        return services;
    }
}