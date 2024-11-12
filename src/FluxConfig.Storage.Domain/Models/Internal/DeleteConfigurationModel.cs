namespace FluxConfig.Storage.Domain.Models.Internal;

public record DeleteConfigurationModel(
    string ConfigurationKey,
    IReadOnlyList<string> ConfigurationTags
    )
{
};