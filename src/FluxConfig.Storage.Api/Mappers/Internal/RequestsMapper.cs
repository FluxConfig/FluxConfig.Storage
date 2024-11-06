using FluxConfig.Storage.Api.GrpcContracts.Internal;
using FluxConfig.Storage.Domain.Models.Internal;
using Google.Protobuf;

namespace FluxConfig.Storage.Api.Mappers.Internal;

public static class RequestsMapper
{
    public static LoadConfigurationModel MapRequestToModel(this LoadConfigRequest request)
    {
        return new LoadConfigurationModel(
            ConfigurationKey: request.ConfigurationKey,
            ConfigurationTag: request.ConfigurationTag
        );
    }

    public static UpdateConfigurationModel MapRequestToModel(this UpdateConfigRequest request)
    {
        return new UpdateConfigurationModel(
            ConfigurationKey: request.ConfigurationKey,
            ConfigurationTag: request.ConfigurationTag,
            RawJsonConfigurationData: JsonFormatter.Default.Format(request.ConfigurationData)
            );
    }

    public static CreateConfigurationModel MapRequestToModel(this CreateServiceConfigRequest request)
    {
        return new CreateConfigurationModel(
            ConfigurationKey: request.ConfigurationKey,
            ConfigurationTag: request.ConfigurationTag
            );
    }

    public static DeleteConfigurationModel MapRequestToModel(this DeleteServiceConfigRequest request)
    {
        return new DeleteConfigurationModel(
            ConfigurationKey: request.ConfigurationKey,
            ConfigurationTag: request.ConfigurationTag
            );
    }
    
}