using FluxConfig.Storage.Api.Clients;
using FluxConfig.Storage.Api.Clients.HttpHandlers;
using FluxConfig.Storage.Api.Clients.Interfaces;
using FluxConfig.Storage.Api.Interceptors;
using FluxConfig.Storage.Api.Services;
using Polly;

namespace FluxConfig.Storage.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGrpcWithInterceptors(this IServiceCollection services,
        IWebHostEnvironment environment)
    {
        services.AddGrpc(options =>
            {
                if (environment.IsDevelopment())
                {
                    options.EnableDetailedErrors = true;
                    options.Interceptors.Add<LoggerInterceptor>();
                }
            })
            .AddServiceOptions<StorageInternalGrpcService>(options =>
            {
                options.Interceptors.Add<Interceptors.Internal.ExceptionHandlerInterceptor>();
            })
            .AddServiceOptions<StoragePublicGrpcService>(options =>
            {
                options.Interceptors.Add<Interceptors.Public.ExceptionHandlerInterceptor>();
                options.Interceptors.Add<Interceptors.Public.ApiKeyAuthInterceptor>();
            });

        return services;
    }

    public static IServiceCollection AddAuthClient(this IServiceCollection services)
    {
        services.AddSingleton<IManagementServiceClient, ManagementServiceClient>();
        services.AddTransient<InternalAuthHeaderHandler>();

        services.AddHttpClient(ManagementServiceClient.ManagementClientTag,
            httpClient =>
            {
                httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("FC_MANAGEMENT_URL") ??
                                                 throw new ArgumentException("Management service url address is missing."));
            })
            .AddHttpMessageHandler<InternalAuthHeaderHandler>()
            .AddTransientHttpErrorPolicy(builder =>
            builder.WaitAndRetryAsync(3, retryTime => TimeSpan.FromMilliseconds(500)));
        
        return services;
    }
}