using FluxConfig.Storage.Api.GrpcContracts.Public;
using FluxConfig.Storage.Domain.Models.Public;
using Grpc.Core;

namespace FluxConfig.Storage.Api.Mappers.Public;

public static class RequestsMapper
{
    public static LoadConfigurationModel MapRequestToModel(this LoadConfigRequest request, ServerCallContext callContext)
    {
        //TODO: REWORK, resolver key from auth service, store logic in auth attribute/interceptor
        return new LoadConfigurationModel(
            ConfigurationKey: string.Equals("TEST-API-KEY", callContext.RequestHeaders.GetValue("X-API-KEY") ?? "", StringComparison.Ordinal)
            ? "TEST-CONFIG-KEY" : "Imposter:D",
            ConfigurationTag: request.ConfigurationTag
        );
    }
}