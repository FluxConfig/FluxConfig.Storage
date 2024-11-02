using FluxConfig.Storage.Infrastructure.Configuration.Models;
using FluxConfig.Storage.Infrastructure.Dal.Infrastructure;
using FluxConfig.Storage.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Storage.Infrastructure.Dal.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluxConfig.Storage.Infrastructure.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDalInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoOptionsSection = configuration.GetSection($"DalOptions:{nameof(MongoDbOptions)}");
        MongoDbOptions mongoOptions = mongoOptionsSection.Get<MongoDbOptions>() ?? throw new ArgumentException("MongoDBOptions is missing");
        
        Mongo.AddMongoDbClient(
            services: services,
            mongoOptions: mongoOptions);
        
        Mongo.AddMigrations(services);
        
        return services;
    }

    public static IServiceCollection AddDalRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRealTimeConfigurationRepository, RealTimeConfigurationRepository>();
        services.AddScoped<IVaultConfigurationRepository, VaultConfigurationRepository>();

        return services;
    }
}