using FluentValidation;
using FluxConfig.Storage.Domain.Exceptions.Domain;
using FluxConfig.Storage.Domain.Models.Public;
using FluxConfig.Storage.Domain.Services.Interfaces;
using FluxConfig.Storage.Domain.Validators.Public;
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
        try
        {
            return await GetVaultConfigurationDataUnsafe(loadConfigModel, cancellationToken);
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "Invalid passed data");
            throw new DomainValidationException("Invalid passed data", ex);
        }
    }

    private async Task<ConfigurationDataModel> GetVaultConfigurationDataUnsafe(LoadConfigurationModel loadConfigModel,
        CancellationToken cancellationToken)
    {
        await ValidateLoadConfigModel(loadConfigModel, cancellationToken);
        return new ConfigurationDataModel(new Dictionary<string, string?>{{"Vault:TestKey", "TestValue"}});
    }

    public async Task<ConfigurationDataModel> GetRealTimeConfigurationData(LoadConfigurationModel loadConfigModel, CancellationToken cancellationToken)
    {
        try
        {
            return await GetRealTimeConfigurationDataUnsafe(loadConfigModel, cancellationToken);
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "Invalid passed data");
            throw new DomainValidationException("Invalid passed data", ex);
        }
    }
    
    private async Task<ConfigurationDataModel> GetRealTimeConfigurationDataUnsafe(LoadConfigurationModel loadConfigModel, CancellationToken cancellationToken)
    {
        await ValidateLoadConfigModel(loadConfigModel, cancellationToken);
        return new ConfigurationDataModel(new Dictionary<string, string?>{{"RealTime:TestKey", "TestValue"}});
    }

    private static async Task ValidateLoadConfigModel(LoadConfigurationModel model, CancellationToken cancellationToken)
    {
        var validator = new LoadConfigurationModelValidator();
        await validator.ValidateAndThrowAsync(model, cancellationToken);
    }
}