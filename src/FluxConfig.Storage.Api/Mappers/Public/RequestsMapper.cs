using FluxConfig.Storage.Api.GrpcContracts.Public;
using FluxConfig.Storage.Domain.Models.Public;
using Grpc.Core;

namespace FluxConfig.Storage.Api.Mappers.Public;

public static class RequestsMapper
{
    public static LoadConfigurationModel MapRequestToModel(this LoadConfigRequest request, ServerCallContext callContext)
    {
        return new LoadConfigurationModel(
            ServiceApiKey: callContext.RequestHeaders.GetValue("X-API-KEY") ?? "",
            ConfigurationTag: request.ConfigurationTag
        );
    }
}