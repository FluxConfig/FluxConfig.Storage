using FluxConfig.Storage.Domain.Models.Public;
using FluxConfig.Storage.Domain.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FluxConfig.Storage.Domain.Services;

public class StoragePublicService : IStoragePublicService
{
    private readonly ILogger<StoragePublicService> _logger;

    public StoragePublicService(ILogger<StoragePublicService> logger)
    {
        _logger = logger;
    }

    public async Task<ConfigurationDataModel> GetVaultConfigurationData(LoadConfigurationModel loadConfigModel, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(5), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task<ConfigurationDataModel> GetRealTimeConfigurationData(LoadConfigurationModel loadConfigModel, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(5), cancellationToken);
        throw new NotImplementedException();
    }
}