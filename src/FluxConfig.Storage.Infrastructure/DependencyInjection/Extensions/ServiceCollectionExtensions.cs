using FluxConfig.Storage.Infrastructure.Configuration.Models;
using FluxConfig.Storage.Infrastructure.Dal.Infrastructure;
using FluxConfig.Storage.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Storage.Infrastructure.Dal.Repositories;
using FluxConfig.Storage.Infrastructure.ISC.Clients;
using FluxConfig.Storage.Infrastructure.ISC.Clients.HttpHandlers;
using FluxConfig.Storage.Infrastructure.ISC.Clients.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

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
    
    public static IServiceCollection AddFcManagementClient(this IServiceCollection services)
    {
        services.AddSingleton<IManagementServiceClient, ManagementServiceClient>();
        services.AddTransient<InternalAuthHeaderHandler>();

        services.AddHttpClient(ManagementServiceClient.ManagementClientTag,
                httpClient =>
                {
                    httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("FCM_BASE_URL") ??
                                                     throw new ArgumentException("Management service url address is missing."));
                })
            .AddHttpMessageHandler<InternalAuthHeaderHandler>()
            .AddTransientHttpErrorPolicy(builder =>
                builder.WaitAndRetryAsync(3, retryTime => TimeSpan.FromMilliseconds(500)));
        
        return services;
    }
}