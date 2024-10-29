using System.Diagnostics.CodeAnalysis;
using FluxConfig.Storage.Api.Interceptors;

namespace FluxConfig.Storage.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGrpcWithInterceptors(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddGrpc(options =>
        {
            if (environment.IsDevelopment())
            {
                options.EnableDetailedErrors = true;
                options.Interceptors.Add<LoggerInterceptor>();
            }
            options.Interceptors.Add<ExceptionHandlerInterceptor>();
        });

        return services;
    }
}