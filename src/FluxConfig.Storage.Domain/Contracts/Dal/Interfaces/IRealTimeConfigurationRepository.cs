using FluxConfig.Storage.Domain.Contracts.Dal.Containers;
using FluxConfig.Storage.Domain.Contracts.Dal.Entities;

namespace FluxConfig.Storage.Domain.Contracts.Dal.Interfaces;

public interface IRealTimeConfigurationRepository
{
    public Task<ConfigurationDataEntity> LoadConfiguration(string configurationKey, string configurationTag,
        CancellationToken cancellationToken);

    public Task UpdateConfiguration(UpdateConfigurationContainer updateContainer, CancellationToken cancellationToken);
}