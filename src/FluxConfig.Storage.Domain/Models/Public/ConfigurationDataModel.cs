namespace FluxConfig.Storage.Domain.Models.Public;

public record ConfigurationDataModel(
    Dictionary<string, string> ConfigurationData
);