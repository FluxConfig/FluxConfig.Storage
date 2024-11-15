using FluxConfig.Storage.Domain.Contracts.Dal.Containers;
using FluxConfig.Storage.Domain.Contracts.Dal.Entities;
using FluxConfig.Storage.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Storage.Domain.Exceptions.Infrastructure;
using FluxConfig.Storage.Infrastructure.Configuration.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FluxConfig.Storage.Infrastructure.Dal.Repositories;

public class RealTimeConfigurationRepository : BaseRepository, IRealTimeConfigurationRepository
{
    public RealTimeConfigurationRepository(
        IOptionsSnapshot<MongoDbCollectionOptions> options,
        IMongoClient mongoClient) :
        base(collectionOptions: options.Get(MongoDbCollectionOptions.RealTimeTag),
            mongoClient: mongoClient)
    {
    }

    public async Task<ConfigurationDataEntity> LoadConfiguration(string configurationKey, string configurationTag,
        CancellationToken cancellationToken)
    {
        try
        {
            return await LoadConfigurationUnsafe(
                configurationKey: configurationKey,
                configurationTag: configurationTag,
                cancellationToken: cancellationToken
            );
        }
        catch (InvalidOperationException ex)
        {
            throw new EntityNotFoundException("Service configuration not found", configurationTag, ex);
        }
        catch (Exception ex)
        {
            throw new InfrastructureException("Unexpected exception occured during configurations.realtime read", ex);
        }
    }

    private async Task<ConfigurationDataEntity> LoadConfigurationUnsafe(string configurationKey,
        string configurationTag,
        CancellationToken cancellationToken)
    {
        IMongoCollection<ConfigurationDataEntity> configCollection = GetConfigurationCollection();

        var filterBuilder = Builders<ConfigurationDataEntity>.Filter;

        var filter = filterBuilder.And(
            filterBuilder.Eq("key", configurationKey),
            filterBuilder.Eq("tag", configurationTag)
        );

        ConfigurationDataEntity entity = await configCollection.Find(filter).FirstAsync(cancellationToken);

        return entity;
    }

    public async Task UpdateConfiguration(UpdateConfigurationContainer updateContainer,
        CancellationToken cancellationToken)
    {   
        try
        {
            await UpdateConfigurationUnsafe(updateContainer, cancellationToken);
        }
        catch (InvalidOperationException ex)
        {
            throw new EntityNotFoundException("Service configuration not found", updateContainer.ConfigurationTag, ex);
        }
        catch (Exception ex)
        {
            throw new InfrastructureException("Unexpected exception occured during configurations.realtime update", ex);
        }
    }
    
    private async Task UpdateConfigurationUnsafe(UpdateConfigurationContainer updateContainer,
        CancellationToken cancellationToken)
    {
        IMongoCollection<ConfigurationDataEntity> configCollection = GetConfigurationCollection();
        
        var filterBuilder = Builders<ConfigurationDataEntity>.Filter;

        var filter = filterBuilder.And(
            filterBuilder.Eq("key", updateContainer.ConfigurationKey),
            filterBuilder.Eq("tag", updateContainer.ConfigurationTag)
        );

        var update = Builders<ConfigurationDataEntity>.Update.Set("data", updateContainer.ConfigurationData);

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