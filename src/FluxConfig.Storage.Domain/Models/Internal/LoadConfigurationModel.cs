namespace FluxConfig.Storage.Domain.Models.Internal;

public record LoadConfigurationModel(
    string ConfigurationKey,
    string ConfigurationTag
)
{
};