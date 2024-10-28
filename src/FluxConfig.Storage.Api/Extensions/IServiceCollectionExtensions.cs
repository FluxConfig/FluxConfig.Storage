using System.Diagnostics.CodeAnalysis;

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
            }
        });

        return services;
    }
}