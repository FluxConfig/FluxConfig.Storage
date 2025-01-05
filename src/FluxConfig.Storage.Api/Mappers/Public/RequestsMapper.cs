using FluxConfig.Storage.Api.GrpcContracts.Public;
using FluxConfig.Storage.Domain.Models.Public;
using Grpc.Core;

namespace FluxConfig.Storage.Api.Mappers.Public;

public static class RequestsMapper
{
    public static LoadConfigurationModel MapRequestToModel(this LoadConfigRequest request, ServerCallContext callContext)
    {
        return new LoadConfigurationModel(
            ConfigurationKey: callContext.RequestHeaders.GetValue("X-CFG-KEY")!,
            ConfigurationTag: request.ConfigurationTag
        );
    }
}