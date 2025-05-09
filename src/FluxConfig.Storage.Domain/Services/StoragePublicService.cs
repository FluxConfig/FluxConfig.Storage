using FluentValidation;
using FluxConfig.Storage.Domain.Contracts.Dal.Entities;
using FluxConfig.Storage.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Storage.Domain.Exceptions.Domain;
using FluxConfig.Storage.Domain.Exceptions.Infrastructure;
using FluxConfig.Storage.Domain.Models.Public;
using FluxConfig.Storage.Domain.Services.Interfaces;
using FluxConfig.Storage.Domain.Validators.Public;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace FluxConfig.Storage.Domain.Services;

public class StoragePublicService : IStoragePublicService
{
    private readonly IRealTimeConfigurationRepository _realTimeCfgRepository;
    private readonly IVaultConfigurationRepository _vaultRepository;

    public StoragePublicService(
        IRealTimeConfigurationRepository realTimeCfgRepository,
        IVaultConfigurationRepository vaultConfigurationRepository)
    {
        _realTimeCfgRepository = realTimeCfgRepository;
        _vaultRepository = vaultConfigurationRepository;
    }

    public async Task<ConfigurationDataModel> GetVaultConfigurationData(
        LoadConfigurationModel loadConfigModel,
        CancellationToken cancellationToken)
    {
        try
        {
            return await GetVaultConfigurationDataUnsafe(loadConfigModel, cancellationToken);
        }
        catch (ValidationException ex)
        {
            throw new DomainValidationException("Invalid passed data", ex);
        }
        catch (EntityNotFoundException ex)
        {
            throw new DomainNotFoundException("Service configuration not found", ex);
        }
    }

    private async Task<ConfigurationDataModel> GetVaultConfigurationDataUnsafe(
        LoadConfigurationModel loadConfigModel,
        CancellationToken cancellationToken)
    {
        await ValidateLoadConfigModel(loadConfigModel, cancellationToken);

        ConfigurationDataEntity entity = await _vaultRepository.LoadConfiguration(
            configurationKey: loadConfigModel.ConfigurationKey,
            configurationTag: loadConfigModel.ConfigurationTag,
            cancellationToken: cancellationToken
        );

        return new ConfigurationDataModel(
            ConfigurationData: MapBsonToDictionary(entity.ConfigurationData)
        );
    }

    public async Task<ConfigurationDataModel> GetRealTimeConfigurationData(LoadConfigurationModel loadConfigModel,
        CancellationToken cancellationToken)
    {
        try
        {
            return await GetRealTimeConfigurationDataUnsafe(loadConfigModel, cancellationToken);
        }
        catch (ValidationException ex)
        {
            throw new DomainValidationException("Invalid passed data", ex);
        }
        catch (EntityNotFoundException ex)
        {
            throw new DomainNotFoundException("Service configuration not found", ex);
        }
    }

    private async Task<ConfigurationDataModel> GetRealTimeConfigurationDataUnsafe(
        LoadConfigurationModel loadConfigModel, CancellationToken cancellationToken)
    {
        await ValidateLoadConfigModel(loadConfigModel, cancellationToken);

        ConfigurationDataEntity entity = await _realTimeCfgRepository.LoadConfiguration(
            configurationKey: loadConfigModel.ConfigurationKey,
            configurationTag: loadConfigModel.ConfigurationTag,
            cancellationToken: cancellationToken
        );

        return new ConfigurationDataModel(
            ConfigurationData: MapBsonToDictionary(entity.ConfigurationData)
        );
    }

    private static Dictionary<string, string> MapBsonToDictionary(BsonDocument bsonConfigMap)
    {
        string rawJson = bsonConfigMap.ToJson();
        Dictionary<string, string> configMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(rawJson) ??
                                               throw new DomainException(
                                                   "Exception occured during client-service configuration deserialization");
        return configMap;
    }


    private static async Task ValidateLoadConfigModel(LoadConfigurationModel model, CancellationToken cancellationToken)
    {
        var validator = new LoadConfigurationModelValidator();
        await validator.ValidateAndThrowAsync(model, cancellationToken);
    }
}