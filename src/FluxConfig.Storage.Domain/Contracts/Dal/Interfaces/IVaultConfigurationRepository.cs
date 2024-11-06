using FluxConfig.Storage.Domain.Contracts.Dal.Entities;

namespace FluxConfig.Storage.Domain.Contracts.Dal.Interfaces;

public interface IVaultConfigurationRepository
{
    public Task<ConfigurationDataEntity> LoadConfiguration(string configurationKey, string configurationTag,
        CancellationToken cancellationToken);
}