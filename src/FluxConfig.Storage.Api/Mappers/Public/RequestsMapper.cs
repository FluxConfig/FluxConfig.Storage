using FluxConfig.Storage.Api.GrpcContracts.Public;
using FluxConfig.Storage.Domain.Models.Public;

namespace FluxConfig.Storage.Api.Mappers.Public;

public static class RequestsMapper
{
    public static LoadConfigurationModel MapRequestToModel(this LoadConfigRequest request, string serviceApiKey)
    {
        return new LoadConfigurationModel(
            ServiceApiKey: serviceApiKey,
            ConfigurationTag: request.ConfigurationTag
        );
    }
}