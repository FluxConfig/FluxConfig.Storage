using FluxConfig.Storage.Domain.Models.Public;

namespace FluxConfig.Storage.Domain.Services.Interfaces;

public interface IStoragePublicService
{
    public Task<ConfigurationDataModel> GetVaultConfigurationData(LoadConfigurationModel loadConfigModel,
        CancellationToken cancellationToken);

    public Task<ConfigurationDataModel> GetRealTimeConfigurationData(LoadConfigurationModel loadConfigModel,
        CancellationToken cancellationToken);
}