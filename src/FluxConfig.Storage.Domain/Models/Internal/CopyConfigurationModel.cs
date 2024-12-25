namespace FluxConfig.Storage.Domain.Models.Internal;

public record CopyConfigurationModel(
    string ConfigurationKey,
    string SourceConfigurationTag,
    string DestConfigurationTag
);