using FluxConfig.Storage.Api.Services;

namespace FluxConfig.Storage.Api.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void MapGrpcServices(this IEndpointRouteBuilder builder)
    {
        builder.MapGrpcService<StoragePublicGrpcService>();
        builder.MapGrpcService<StorageInternalGrpcService>();
    }
}