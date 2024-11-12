using FluxConfig.Storage.Domain.Contracts.Dal.Entities;

namespace FluxConfig.Storage.Domain.Contracts.Dal.Interfaces;

public interface ISharedConfigurationRepository
{
    public Task CreateNewConfigurationDocument(ConfigurationDataEntity configEntity,
        CancellationToken cancellationToken);

    public Task DeleteConfigurationDocuments(string configurationKey, IReadOnlyList<string> configurationTags,
        CancellationToken cancellationToken);
}