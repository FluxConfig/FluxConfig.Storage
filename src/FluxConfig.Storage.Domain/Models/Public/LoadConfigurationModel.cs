namespace FluxConfig.Storage.Domain.Models.Public;

public record LoadConfigurationModel(
    string ServiceApiKey,
    string ConfigurationTag
) {}