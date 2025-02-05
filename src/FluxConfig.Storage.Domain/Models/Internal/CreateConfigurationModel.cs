namespace FluxConfig.Storage.Domain.Models.Internal;

public record CreateConfigurationModel(
    string ConfigurationKey,
    string ConfigurationTag
)
{
};