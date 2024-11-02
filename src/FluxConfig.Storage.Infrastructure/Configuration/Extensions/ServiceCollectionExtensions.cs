using FluxConfig.Storage.Infrastructure.Configuration.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluxConfig.Storage.Infrastructure.Configuration.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbOptions>(configuration.GetSection($"DalOptions:{nameof(MongoDbOptions)}"));
        
        return services;
    }
}