using FluxConfig.Storage.Api.Interceptors;
using FluxConfig.Storage.Api.Services;

namespace FluxConfig.Storage.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGrpcWithInterceptors(this IServiceCollection services,
        IWebHostEnvironment environment)
    {
        services.AddGrpc(options =>
            {
                options.Interceptors.Add<LoggerInterceptor>();
                
                if (environment.IsDevelopment())
                {
                    options.EnableDetailedErrors = true;
                    options.Interceptors.Add<HeadersLoggerInterceptor>();
                }
            })
            .AddServiceOptions<StorageInternalGrpcService>(options =>
            {
                options.Interceptors.Add<Interceptors.Internal.ExceptionHandlerInterceptor>();
                options.Interceptors.Add<Interceptors.Internal.ApiKeyAuthInterceptor>();
            })
            .AddServiceOptions<StoragePublicGrpcService>(options =>
            {
                options.Interceptors.Add<Interceptors.Public.ExceptionHandlerInterceptor>();
                options.Interceptors.Add<Interceptors.Public.ApiKeyAuthInterceptor>();
            });

        return services;
    }
}