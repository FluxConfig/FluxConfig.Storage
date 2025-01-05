namespace FluxConfig.Storage.Domain.Models.Internal;

public record ChangeConfigTagModel(
    string ConfigurationKey,
    string OldConfigurationTag,
    string NewConfigurationTag
)
{
};