using FluentValidation;
using FluxConfig.Storage.Domain.Contracts.Dal.Containers;
using FluxConfig.Storage.Domain.Contracts.Dal.Entities;
using FluxConfig.Storage.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Storage.Domain.Exceptions.Domain;
using FluxConfig.Storage.Domain.Exceptions.Infrastructure;
using FluxConfig.Storage.Domain.Models.Internal;
using FluxConfig.Storage.Domain.Models.Internal.Mappers;
using FluxConfig.Storage.Domain.Services.Interfaces;
using FluxConfig.Storage.Domain.Validators.Internal;

namespace FluxConfig.Storage.Domain.Services;

public class StorageInternalService : IStorageInternalService
{
    private readonly IVaultConfigurationRepository _vaultRepository;
    private readonly IRealTimeConfigurationRepository _realTimeRepository;
    private readonly ISharedConfigurationRepository _sharedRepository;

    public StorageInternalService(
        IVaultConfigurationRepository vaultConfigurationRepository,
        IRealTimeConfigurationRepository realTimeConfigurationRepository,
        ISharedConfigurationRepository sharedConfigurationRepository)
    {
        _vaultRepository = vaultConfigurationRepository;
        _realTimeRepository = realTimeConfigurationRepository;
        _sharedRepository = sharedConfigurationRepository;
    }

    public async Task<ConfigurationDataModel> GetVaultConfigurationData(LoadConfigurationModel model,
        CancellationToken cancellationToken)
    {
        try
        {
            return await GetVaultConfigurationDataUnsafe(model, cancellationToken);
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

    private async Task<ConfigurationDataModel> GetVaultConfigurationDataUnsafe(LoadConfigurationModel model,
        CancellationToken cancellationToken)
    {
        var validator = new LoadConfigurationModelValidator();
        await validator.ValidateAndThrowAsync(model, cancellationToken);

        ConfigurationDataEntity entity = await _vaultRepository.LoadConfiguration(
            configurationKey: model.ConfigurationKey,
            configurationTag: model.ConfigurationTag,
            cancellationToken: cancellationToken
        );

        return entity.MapEntityToModel();
    }

    public async Task<ConfigurationDataModel> GetRealTimeConfigurationData(LoadConfigurationModel model,
        CancellationToken cancellationToken)
    {
        try
        {
            return await GetRealTimeConfigurationDataUnsafe(model, cancellationToken);
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

    private async Task<ConfigurationDataModel> GetRealTimeConfigurationDataUnsafe(LoadConfigurationModel model,
        CancellationToken cancellationToken)
    {
        var validator = new LoadConfigurationModelValidator();
        await validator.ValidateAndThrowAsync(model, cancellationToken);

        ConfigurationDataEntity entity = await _realTimeRepository.LoadConfiguration(
            configurationKey: model.ConfigurationKey,
            configurationTag: model.ConfigurationTag,
            cancellationToken: cancellationToken
        );

        return entity.MapEntityToModel();
    }

    public async Task UpdateVaultConfiguration(UpdateConfigurationModel model, CancellationToken cancellationToken)
    {
        try
        {
            await UpdateVaultConfigurationUnsafe(model, cancellationToken);
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

    private async Task UpdateVaultConfigurationUnsafe(UpdateConfigurationModel model,
        CancellationToken cancellationToken)
    {
        var validator = new UpdateConfigurationModelValidator();
        await validator.ValidateAndThrowAsync(model, cancellationToken);

        await _vaultRepository.UpdateConfiguration(
            updateContainer: model.MapModelToContainer(),
            cancellationToken: cancellationToken
        );
    }

    public async Task UpdateRealTimeConfiguration(UpdateConfigurationModel model, CancellationToken cancellationToken)
    {
        try
        {
            await UpdateRealTimeConfigurationUnsafe(model, cancellationToken);
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

    private async Task UpdateRealTimeConfigurationUnsafe(UpdateConfigurationModel model,
        CancellationToken cancellationToken)
    {
        var validator = new UpdateConfigurationModelValidator();
        await validator.ValidateAndThrowAsync(model, cancellationToken);

        await _realTimeRepository.UpdateConfiguration(
            updateContainer: model.MapModelToContainer(),
            cancellationToken: cancellationToken
        );
    }

    public async Task CreateNewServiceConfiguration(CreateConfigurationModel model, CancellationToken cancellationToken)
    {
        try
        {
            await CreateNewServiceConfigurationUnsafe(model, cancellationToken);
        }
        catch (ValidationException ex)
        {
            throw new DomainValidationException("Invalid passed data", ex);
        }
        catch (EntityAlreadyExistsException exception)
        {
            throw new DomainAlreadyExistsException("Duplicate configuration creation", exception);
        }
    }

    private async Task CreateNewServiceConfigurationUnsafe(CreateConfigurationModel model,
        CancellationToken cancellationToken)
    {
        var validator = new CreateConfigurationModelValidator();
        await validator.ValidateAndThrowAsync(model, cancellationToken);

        await _sharedRepository.CreateNewConfigurationDocument(
            configEntity: model.MapModelToEntity(),
            cancellationToken: cancellationToken
        );
    }

    public async Task DeleteServiceConfiguration(DeleteConfigurationModel model, CancellationToken cancellationToken)
    {
        try
        {
            await DeleteServiceConfigurationUnsafe(model, cancellationToken);
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

    private async Task DeleteServiceConfigurationUnsafe(DeleteConfigurationModel model,
        CancellationToken cancellationToken)
    {
        var validator = new DeleteConfigurationModelValidator();
        await validator.ValidateAndThrowAsync(model, cancellationToken);

        await _sharedRepository.DeleteConfigurationDocuments(
            configurationKey: model.ConfigurationKey,
            configurationTags: model.ConfigurationTags,
            cancellationToken: cancellationToken
        );
    }

    public async Task ChangeServiceConfigTag(ChangeConfigTagModel model, CancellationToken cancellationToken)
    {
        try
        {
            await ChangeServiceConfigTagUnsafe(model, cancellationToken);
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


    private async Task ChangeServiceConfigTagUnsafe(ChangeConfigTagModel model, CancellationToken cancellationToken)
    {
        var validator = new ChangeConfigTagModelValidator();
        await validator.ValidateAndThrowAsync(model, cancellationToken);

        await _sharedRepository.ChangeServiceConfigurationTag(
            changeTagContainer: model.MapModelToContainer(),
            cancellationToken: cancellationToken
        );
    }

    public async Task CopyRealTimeConfigData(CopyConfigurationModel model, CancellationToken cancellationToken)
    {
        try
        {
            await CopyRealTimeConfigDataUnsafe(model, cancellationToken);
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

    private async Task CopyRealTimeConfigDataUnsafe(CopyConfigurationModel model, CancellationToken cancellationToken)
    {
        var validator = new CopyConfigurationModelValidator();
        await validator.ValidateAndThrowAsync(model, cancellationToken);

        ConfigurationDataEntity sourceEntity = await _realTimeRepository.LoadConfiguration(
            configurationKey: model.ConfigurationKey,
            configurationTag: model.SourceConfigurationTag,
            cancellationToken: cancellationToken
        );

        await _realTimeRepository.UpdateConfiguration(
            updateContainer: new UpdateConfigurationContainer(
                ConfigurationKey: model.ConfigurationKey,
                ConfigurationTag: model.DestConfigurationTag,
                ConfigurationData: sourceEntity.ConfigurationData
            ),
            cancellationToken: cancellationToken
        );
    }

    public async Task CopyVaultConfigData(CopyConfigurationModel model, CancellationToken cancellationToken)
    {
        try
        {
            await CopyVaultConfigDataUnsafe(model, cancellationToken);
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

    private async Task CopyVaultConfigDataUnsafe(CopyConfigurationModel model, CancellationToken cancellationToken)
    {
        var validator = new CopyConfigurationModelValidator();
        await validator.ValidateAndThrowAsync(model, cancellationToken);

        ConfigurationDataEntity sourceEntity = await _vaultRepository.LoadConfiguration(
            configurationKey: model.ConfigurationKey,
            configurationTag: model.SourceConfigurationTag,
            cancellationToken: cancellationToken
        );

        await _vaultRepository.UpdateConfiguration(
            updateContainer: new UpdateConfigurationContainer(
                ConfigurationKey: model.ConfigurationKey,
                ConfigurationTag: model.DestConfigurationTag,
                ConfigurationData: sourceEntity.ConfigurationData
            ),
            cancellationToken: cancellationToken
        );
    }
}