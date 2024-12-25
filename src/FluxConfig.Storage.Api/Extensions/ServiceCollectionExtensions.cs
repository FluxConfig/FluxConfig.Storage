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
            if (environment.IsDevelopment())
            {
                options.EnableDetailedErrors = true;
                options.Interceptors.Add<LoggerInterceptor>();
            }
        })
        .AddServiceOptions<StorageInternalGrpcService>(options =>
        {
            options.Interceptors.Add<InternalExceptionHandlerInterceptor>();
        })
        .AddServiceOptions<StoragePublicGrpcService>(options =>
        {
            options.Interceptors.Add<PublicExceptionHandlerInterceptor>();
        });

        return services;
    }
}