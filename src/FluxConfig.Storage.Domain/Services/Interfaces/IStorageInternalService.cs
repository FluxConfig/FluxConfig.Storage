using FluxConfig.Storage.Domain.Models.Internal;

namespace FluxConfig.Storage.Domain.Services.Interfaces;

public interface IStorageInternalService
{
    public Task<ConfigurationDataModel> GetVaultConfigurationData(LoadConfigurationModel model,
        CancellationToken cancellationToken);

    public Task<ConfigurationDataModel> GetRealTimeConfigurationData(LoadConfigurationModel model,
        CancellationToken cancellationToken);

    public Task UpdateVaultConfiguration(UpdateConfigurationModel model, CancellationToken cancellationToken);

    public Task UpdateRealTimeConfiguration(UpdateConfigurationModel model, CancellationToken cancellationToken);

    public Task CreateNewServiceConfiguration(CreateConfigurationModel model, CancellationToken cancellationToken);

    public Task DeleteServiceConfiguration(DeleteConfigurationModel model, CancellationToken cancellationToken);
}