using FluxConfig.Storage.Domain.Models.Internal;
using FluxConfig.Storage.Domain.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FluxConfig.Storage.Domain.Services;

public class StorageInternalService: IStorageInternalService
{
    private readonly ILogger<StorageInternalService> _logger;

    public StorageInternalService(ILogger<StorageInternalService> logger)
    {
        _logger = logger;
    }
    
    public async Task<ConfigurationDataModel> GetVaultConfigurationData(LoadConfigurationModel model, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMicroseconds(10), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task<ConfigurationDataModel> GetRealTimeConfigurationData(LoadConfigurationModel model, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMicroseconds(10), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task UpdateVaultConfiguration(UpdateConfigurationModel model, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMicroseconds(10), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task UpdateRealTimeConfiguration(UpdateConfigurationModel model, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMicroseconds(10), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task CreateNewServiceConfiguration(CreateConfigurationModel model, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMicroseconds(10), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task DeleteServiceConfiguration(DeleteConfigurationModel model, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMicroseconds(10), cancellationToken);
        throw new NotImplementedException();
    }
}