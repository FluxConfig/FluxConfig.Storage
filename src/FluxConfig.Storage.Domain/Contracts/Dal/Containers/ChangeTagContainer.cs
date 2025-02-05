namespace FluxConfig.Storage.Domain.Contracts.Dal.Containers;

public record ChangeTagContainer(
    string ConfigurationKey,
    string OldConfigTag,
    string NewConfigTag
)
{
}