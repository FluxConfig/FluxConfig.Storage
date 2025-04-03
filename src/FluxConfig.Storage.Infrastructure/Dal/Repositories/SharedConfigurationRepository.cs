using FluxConfig.Storage.Domain.Contracts.Dal.Containers;
using FluxConfig.Storage.Domain.Contracts.Dal.Entities;
using FluxConfig.Storage.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Storage.Domain.Exceptions.Infrastructure;
using FluxConfig.Storage.Domain.Extensions;
using FluxConfig.Storage.Infrastructure.Configuration.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FluxConfig.Storage.Infrastructure.Dal.Repositories;

public class SharedConfigurationRepository : BaseRepository, ISharedConfigurationRepository
{
    public SharedConfigurationRepository(
        IOptionsSnapshot<MongoDbCollectionOptions> optionsSnapshot,
        IMongoClient mongoClient)
        : base(collectionOptions: optionsSnapshot.Get(MongoDbCollectionOptions.VaultTag),
            mongoClient: mongoClient)
    {
    }

    public async Task CreateNewConfigurationDocument(ConfigurationDataEntity configEntity,
        CancellationToken cancellationToken)
    {
        try
        {
            await CreateNewConfigurationDocumentUnsafe(configEntity, cancellationToken);
        }
        catch (AggregateException ex)
        {
            List<MongoWriteException> duplicateExceptions =
                ex.InnerExceptions.OfType<MongoWriteException>()
                    .Where(exc => exc.WriteError.Category == ServerErrorCategory.DuplicateKey).ToList();

            if (duplicateExceptions.Count > 0)
            {
                throw new EntityAlreadyExistsException("Configuration entity already exists",
                    configEntity.ConfigurationKey, configEntity.ConfigurationTag, duplicateExceptions[0]);
            }

            throw new InfrastructureException("Exception occured during configurations creation", ex);
        }
    }

    private async Task CreateNewConfigurationDocumentUnsafe(ConfigurationDataEntity configEntity,
        CancellationToken cancellationToken)
    {
        var createTasks = new List<Task>
        {
            CreateNewVaultConfigurationDocument(configEntity, cancellationToken),
            CreateNewRealTimeConfigurationDocument(configEntity, cancellationToken)
        };

        await TaskExt.WhenAll(createTasks);
    }

    private async Task CreateNewVaultConfigurationDocument(ConfigurationDataEntity newConfigEntity,
        CancellationToken cancellationToken)
    {
        IMongoDatabase configDb = GetConfigurationDatabase();
        IMongoCollection<ConfigurationDataEntity> vaultConfigCollection =
            configDb.GetCollection<ConfigurationDataEntity>(MongoDbCollectionOptions.VaultTag.ToLower());

        await vaultConfigCollection.InsertOneAsync(
            document: newConfigEntity,
            options: new InsertOneOptions()
            {
                BypassDocumentValidation = false
            },
            cancellationToken: cancellationToken
        );
    }

    private async Task CreateNewRealTimeConfigurationDocument(ConfigurationDataEntity newConfigEntity,
        CancellationToken cancellationToken)
    {
        IMongoDatabase configDb = GetConfigurationDatabase();
        IMongoCollection<ConfigurationDataEntity> vaultConfigCollection =
            configDb.GetCollection<ConfigurationDataEntity>(MongoDbCollectionOptions.RealTimeTag.ToLower());

        await vaultConfigCollection.InsertOneAsync(
            document: newConfigEntity,
            options: new InsertOneOptions()
            {
                BypassDocumentValidation = false
            },
            cancellationToken: cancellationToken
        );
    }

    public async Task DeleteConfigurationDocuments(string configurationKey, IReadOnlyList<string> configurationTags,
        CancellationToken cancellationToken)
    {
        try
        {
            await DeleteConfigurationDocumentsUnsafe(configurationKey, configurationTags, cancellationToken);
        }
        catch (AggregateException ex)
        {
            if (ex.InnerExceptions.Count(e => e is InvalidOperationException) > 0)
            {
                throw new EntityNotFoundException("Service configuration not found", configurationTags[0],
                    configurationKey, ex);
            }

            throw new InfrastructureException("Exception occured during configurations deletion", ex);
        }
    }

    private async Task DeleteConfigurationDocumentsUnsafe(
        string configurationKey,
        IReadOnlyList<string> configurationTags,
        CancellationToken cancellationToken)
    {
        var deleteOperations = ConvertToDeletionOperations(configurationKey, configurationTags);

        var tasks = new List<Task>
        {
            DeleteVaultConfigurations(
                deleteOperations: deleteOperations,
                cancellationToken: cancellationToken
            ),
            DeleteRealTimeConfigurations(
                deleteOperations: deleteOperations,
                cancellationToken: cancellationToken
            )
        };

        await TaskExt.WhenAll(tasks);
    }

    private WriteModel<ConfigurationDataEntity>[] ConvertToDeletionOperations(string configurationKey,
        IReadOnlyList<string> configurationTags)
    {
        return configurationTags.Select(tag =>
            {
                var filterBuilder = Builders<ConfigurationDataEntity>.Filter;
                return new DeleteOneModel<ConfigurationDataEntity>(
                    filter: filterBuilder.And(
                        filterBuilder.Eq("key", configurationKey),
                        filterBuilder.Eq("tag", tag)
                    )
                );
            }
        ).ToArray<WriteModel<ConfigurationDataEntity>>();
    }


    private async Task DeleteVaultConfigurations(
        WriteModel<ConfigurationDataEntity>[] deleteOperations, CancellationToken cancellationToken)
    {
        IMongoDatabase configDb = GetConfigurationDatabase();
        IMongoCollection<ConfigurationDataEntity> configCollection =
            configDb.GetCollection<ConfigurationDataEntity>(MongoDbCollectionOptions.VaultTag.ToLower());

        var result = await configCollection.BulkWriteAsync(
            requests: deleteOperations,
            cancellationToken: cancellationToken);

        if (result.IsAcknowledged == false || result.DeletedCount != deleteOperations.Length)
        {
            throw new InvalidOperationException();
        }
    }

    private async Task DeleteRealTimeConfigurations(
        WriteModel<ConfigurationDataEntity>[] deleteOperations, CancellationToken cancellationToken)
    {
        IMongoDatabase configDb = GetConfigurationDatabase();
        IMongoCollection<ConfigurationDataEntity> configCollection =
            configDb.GetCollection<ConfigurationDataEntity>(MongoDbCollectionOptions.RealTimeTag.ToLower());

        var result = await configCollection.BulkWriteAsync(
            requests: deleteOperations,
            cancellationToken: cancellationToken);

        if (result.IsAcknowledged == false || result.DeletedCount != deleteOperations.Length)
        {
            throw new InvalidOperationException();
        }
    }

    public async Task ChangeServiceConfigurationTag(ChangeTagContainer changeTagContainer,
        CancellationToken cancellationToken)
    {
        try
        {
            await ChangeServiceConfigurationTagUnsafe(changeTagContainer, cancellationToken);
        }
        catch (AggregateException ex)
        {
            if (ex.InnerExceptions.Count(e => e is InvalidOperationException) > 0)
            {
                throw new EntityNotFoundException("Service configuration not found", changeTagContainer.OldConfigTag,
                    changeTagContainer.ConfigurationKey, ex);
            }

            throw new InfrastructureException("Exception occured during configurations deletion", ex);
        }
    }

    private async Task ChangeServiceConfigurationTagUnsafe(ChangeTagContainer changeTagContainer,
        CancellationToken cancellationToken)
    {
        var tasks = new List<Task>
        {
            ChangeVaultConfigurationTag(changeTagContainer, cancellationToken),
            ChangeRealTimeConfigurationTag(changeTagContainer, cancellationToken)
        };

        await TaskExt.WhenAll(tasks);
    }

    private async Task ChangeVaultConfigurationTag(ChangeTagContainer container, CancellationToken cancellationToken)
    {
        IMongoDatabase configDb = GetConfigurationDatabase();
        IMongoCollection<ConfigurationDataEntity> configCollection =
            configDb.GetCollection<ConfigurationDataEntity>(MongoDbCollectionOptions.VaultTag.ToLower());

        var filterBuilder = Builders<ConfigurationDataEntity>.Filter;

        var filter = filterBuilder.And(
            filterBuilder.Eq("key", container.ConfigurationKey),
            filterBuilder.Eq("tag", container.OldConfigTag)
        );

        var update = Builders<ConfigurationDataEntity>.Update.Set("tag", container.NewConfigTag);

        var result = await configCollection.UpdateOneAsync(
            filter: filter,
            update: update,
            cancellationToken: cancellationToken
        );

        if (!result.IsAcknowledged || result.MatchedCount <= 0)
        {
            throw new InvalidOperationException();
        }
    }

    private async Task ChangeRealTimeConfigurationTag(ChangeTagContainer container, CancellationToken cancellationToken)
    {
        IMongoDatabase configDb = GetConfigurationDatabase();
        IMongoCollection<ConfigurationDataEntity> configCollection =
            configDb.GetCollection<ConfigurationDataEntity>(MongoDbCollectionOptions.RealTimeTag.ToLower());

        var filterBuilder = Builders<ConfigurationDataEntity>.Filter;

        var filter = filterBuilder.And(
            filterBuilder.Eq("key", container.ConfigurationKey),
            filterBuilder.Eq("tag", container.OldConfigTag)
        );

        var update = Builders<ConfigurationDataEntity>.Update.Set("tag", container.NewConfigTag);

        var result = await configCollection.UpdateOneAsync(
            filter: filter,
            update: update,
            cancellationToken: cancellationToken
        );

        if (!result.IsAcknowledged || result.MatchedCount <= 0)
        {
            throw new InvalidOperationException();
        }
    }
}