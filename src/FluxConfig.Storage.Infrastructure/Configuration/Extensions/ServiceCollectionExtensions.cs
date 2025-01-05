using FluxConfig.Storage.Infrastructure.Configuration.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluxConfig.Storage.Infrastructure.Configuration.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureConfigurationOptions(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MongoDbConnectionOptions>(
            configuration.GetSection($"DalOptions:{nameof(MongoDbConnectionOptions)}"));

        services.Configure<MongoDbCollectionOptions>(MongoDbCollectionOptions.VaultTag,
            configuration.GetSection(
                $"DalOptions:{nameof(MongoDbCollectionOptions)}:{MongoDbCollectionOptions.VaultTag}"));

        services.Configure<MongoDbCollectionOptions>(MongoDbCollectionOptions.RealTimeTag,
            configuration.GetSection(
                $"DalOptions:{nameof(MongoDbCollectionOptions)}:{MongoDbCollectionOptions.RealTimeTag}"));

        return services;
    }
}