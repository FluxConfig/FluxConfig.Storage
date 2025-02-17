using FluentValidation;
using FluxConfig.Storage.Domain.Contracts.Dal.Containers;
using FluxConfig.Storage.Domain.Contracts.Dal.Entities;
using FluxConfig.Storage.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Storage.Domain.Exceptions.Domain;
using FluxConfig.Storage.Domain.Exceptions.Infrastructure;
using FluxConfig.Storage.Domain.Extensions;
using FluxConfig.Storage.Domain.Models.Internal;
using FluxConfig.Storage.Domain.Models.Internal.Mappers;
using FluxConfig.Storage.Domain.Services.Interfaces;
using FluxConfig.Storage.Domain.Validators.Internal;
using Microsoft.Extensions.Logging;

namespace FluxConfig.Storage.Domain.Services;

public class StorageInternalService : IStorageInternalService
{
    private readonly IVaultConfigurationRepository _vaultRepository;
    private readonly IRealTimeConfigurationRepository _realTimeRepository;
    private readonly ISharedConfigurationRepository _sharedRepository;
    private readonly ILogger<StorageInternalService> _logger;

    public StorageInternalService(
        IVaultConfigurationRepository vaultConfigurationRepository,
        IRealTimeConfigurationRepository realTimeConfigurationRepository,
        ISharedConfigurationRepository sharedConfigurationRepository,
        ILogger<StorageInternalService> logger)
    {
        _vaultRepository = vaultConfigurationRepository;
        _realTimeRepository = realTimeConfigurationRepository;
        _sharedRepository = sharedConfigurationRepository;
        _logger = logger;
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
            _logger.LogDomainBadRequestError(
                curTime: DateTime.Now,
                tag: model.ConfigurationTag,
                key: model.ConfigurationKey,
                exception: ex
            );
            throw new DomainValidationException("Invalid passed data", ex);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogDomainNotFoundError(
                curTime: DateTime.Now,
                tag: ex.ConfigurationTag,
                key: ex.ConfigurationKey,
                exception: ex
            );
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
            _logger.LogDomainBadRequestError(
                curTime: DateTime.Now,
                tag: model.ConfigurationTag,
                key: model.ConfigurationKey,
                exception: ex
            );
            throw new DomainValidationException("Invalid passed data", ex);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogDomainNotFoundError(
                curTime: DateTime.Now,
                tag: ex.ConfigurationTag,
                key: ex.ConfigurationKey,
                exception: ex
            );
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
            _logger.LogDomainBadRequestError(
                curTime: DateTime.Now,
                tag: model.ConfigurationTag,
                key: model.ConfigurationKey,
                exception: ex
            );

            throw new DomainValidationException("Invalid passed data", ex);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogDomainNotFoundError(
                curTime: DateTime.Now,
                tag: ex.ConfigurationTag,
                key: ex.ConfigurationKey,
                exception: ex
            );
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
            _logger.LogDomainBadRequestError(
                curTime: DateTime.Now,
                tag: model.ConfigurationTag,
                key: model.ConfigurationKey,
                exception: ex
            );
            throw new DomainValidationException("Invalid passed data", ex);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogDomainNotFoundError(
                curTime: DateTime.Now,
                tag: ex.ConfigurationTag,
                key: ex.ConfigurationKey,
                exception: ex
            );
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
            _logger.LogDomainBadRequestError(
                curTime: DateTime.Now,
                tag: model.ConfigurationTag,
                key: model.ConfigurationKey,
                exception: ex
            );
            throw new DomainValidationException("Invalid passed data", ex);
        }
        catch (EntityAlreadyExistsException exception)
        {
            _logger.LogDomainDuplicateConfigError(
                curTime: DateTime.Now,
                key: model.ConfigurationKey,
                tag: exception.ConfigurationTag,
                exception: exception
            );
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
            _logger.LogDomainBadRequestError(
                curTime: DateTime.Now,
                tag: model.ConfigurationTags[0],
                key: model.ConfigurationKey,
                exception: ex
            );

            throw new DomainValidationException("Invalid passed data", ex);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogDomainNotFoundError(
                curTime: DateTime.Now,
                tag: ex.ConfigurationTag,
                key: ex.ConfigurationKey,
                exception: ex
            );
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
            _logger.LogDomainBadRequestError(
                curTime: DateTime.Now,
                tag: model.OldConfigurationTag,
                key: model.ConfigurationKey,
                exception: ex
            );

            throw new DomainValidationException("Invalid passed data", ex);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogDomainNotFoundError(
                curTime: DateTime.Now,
                tag: ex.ConfigurationTag,
                key: ex.ConfigurationKey,
                exception: ex
            );
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
            _logger.LogDomainBadRequestError(
                curTime: DateTime.Now,
                tag: model.SourceConfigurationTag,
                key: model.ConfigurationKey,
                exception: ex
            );
            throw new DomainValidationException("Invalid passed data", ex);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogDomainNotFoundError(
                curTime: DateTime.Now,
                tag: ex.ConfigurationTag,
                key: ex.ConfigurationKey,
                exception: ex
            );
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
            _logger.LogDomainBadRequestError(
                curTime: DateTime.Now,
                tag: model.DestConfigurationTag,
                key: model.ConfigurationKey,
                exception: ex
            );
            throw new DomainValidationException("Invalid passed data", ex);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogDomainNotFoundError(
                curTime: DateTime.Now,
                tag: ex.ConfigurationTag,
                key: ex.ConfigurationKey,
                exception: ex
            );
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