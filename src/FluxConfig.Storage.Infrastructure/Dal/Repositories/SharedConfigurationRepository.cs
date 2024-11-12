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
            if (ex.InnerExceptions[0] is MongoWriteException &&
                ((MongoWriteException)ex.InnerExceptions[0]).WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                throw new EntityAlreadyExistsException("Configuration entity already exists",
                    configEntity.ConfigurationTag, ex.InnerExceptions[0]);
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
            configDb.GetCollection<ConfigurationDataEntity>("vault");

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
            configDb.GetCollection<ConfigurationDataEntity>("realtime");

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
        await Task.Delay(TimeSpan.FromMilliseconds(5), cancellationToken);
        throw new NotImplementedException();
    }
}