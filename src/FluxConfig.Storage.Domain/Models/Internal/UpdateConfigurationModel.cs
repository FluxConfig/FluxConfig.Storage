namespace FluxConfig.Storage.Domain.Models.Internal;

public record UpdateConfigurationModel(
    string ConfigurationKey,
    string ConfigurationTag,
    string RawJsonConfigurationData
)
{
};