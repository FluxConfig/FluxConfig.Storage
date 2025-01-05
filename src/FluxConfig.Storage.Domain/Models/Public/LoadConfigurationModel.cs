namespace FluxConfig.Storage.Domain.Models.Public;

public record LoadConfigurationModel(
    string ConfigurationKey,
    string ConfigurationTag
) {}