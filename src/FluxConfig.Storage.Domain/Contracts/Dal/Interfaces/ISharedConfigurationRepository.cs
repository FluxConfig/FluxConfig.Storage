namespace FluxConfig.Storage.Domain.Contracts.Dal.Interfaces;

public interface ISharedConfigurationRepository
{
    public Task CreateNewConfigurationDocument(string configurationKey, string configurationTag,
        CancellationToken cancellationToken);

    public Task DeleteConfigurationDocument(string configurationKey, IReadOnlyList<string> configurationTags,
        CancellationToken cancellationToken);
}