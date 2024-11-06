namespace FluxConfig.Storage.Domain.Models.Internal;

public record DeleteConfigurationModel(
    string ConfigurationKey,
    string ConfigurationTag)
{
};