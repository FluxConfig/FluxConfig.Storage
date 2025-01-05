using FluxConfig.Storage.Api.GrpcContracts.Internal;
using FluxConfig.Storage.Domain.Models.Internal;
using Google.Protobuf.WellKnownTypes;

namespace FluxConfig.Storage.Api.Mappers.Internal;

public static class ResponsesMapper
{
    public static LoadConfigResponse MapModelToResponse(this ConfigurationDataModel model)
    {
        return new LoadConfigResponse()
        {
            ConfigurationData = Value.Parser.ParseJson(model.RawJsonConfigurationData)
        };
    }
}