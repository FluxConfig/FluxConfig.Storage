using FluxConfig.Storage.Infrastructure.Configuration.Models;
using FluxConfig.Storage.Infrastructure.Dal.Infrastructure;
using FluxConfig.Storage.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Storage.Infrastructure.Dal.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluxConfig.Storage.Infrastructure.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDalInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        var mongoConnectionOptionsSection = configuration.GetSection($"DalOptions:{nameof(MongoDbConnectionOptions)}");
        MongoDbConnectionOptions mongoOptions = mongoConnectionOptionsSection.Get<MongoDbConnectionOptions>() ??
                                                throw new ArgumentException("MongoDBConnectionOptions is missing");
    
        MongoDb.MapComplexTypes();
        
        MongoDb.AddClient(
            services: services,
            mongoOptions: mongoOptions);

        return services;
    }

    public static IServiceCollection AddDalRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRealTimeConfigurationRepository, RealTimeConfigurationRepository>();
        services.AddScoped<IVaultConfigurationRepository, VaultConfigurationRepository>();
        services.AddScoped<ISharedConfigurationRepository, SharedConfigurationRepository>();

        return services;
    }
}