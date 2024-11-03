using FluxConfig.Storage.Infrastructure.Configuration.Models;
using FluxConfig.Storage.Infrastructure.Dal.Repositories.Interfaces;
using MongoDB.Driver;

namespace FluxConfig.Storage.Infrastructure.Dal.Repositories;

public abstract class BaseRepository : IDbRepository
{
    private readonly IMongoClient _mongoClient;
    private readonly MongoDbCollectionOptions _mongoCollectionOptions;

    protected BaseRepository(MongoDbCollectionOptions collectionOptions, IMongoClient mongoClient)
    {
        _mongoCollectionOptions = collectionOptions;
        _mongoClient = mongoClient;
    }

    protected IMongoDatabase GetConfigurationDatabase()
    {
        return _mongoClient.GetDatabase(_mongoCollectionOptions.DatabaseName);
    }
    
    public async Task<IClientSession> CreateSessionAsync(CancellationToken cancellationToken)
    {
        return await _mongoClient.StartSessionAsync(
            options: null,
            cancellationToken: cancellationToken);
    }
}