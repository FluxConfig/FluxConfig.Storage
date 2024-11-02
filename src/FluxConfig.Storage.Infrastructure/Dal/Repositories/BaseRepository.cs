using FluxConfig.Storage.Domain.Contracts.Dal.Entities;
using FluxConfig.Storage.Infrastructure.Configuration.Models;
using FluxConfig.Storage.Infrastructure.Dal.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FluxConfig.Storage.Infrastructure.Dal.Repositories;

public abstract class BaseRepository : IDbRepository
{
    private readonly IMongoClient _mongoClient;
    private readonly MongoDbOptions _mongoOptions;

    protected BaseRepository(IOptions<MongoDbOptions> mongoOptions, IMongoClient mongoClient)
    {
        _mongoOptions = mongoOptions.Value;
        _mongoClient = mongoClient;
    }

    protected IMongoDatabase GetConfigurationDatabase()
    {
        return _mongoClient.GetDatabase(_mongoOptions.DatabaseName);
    }
    
    public async Task<IClientSession> CreateSessionAsync(CancellationToken cancellationToken)
    {
        return await _mongoClient.StartSessionAsync(
            options: null,
            cancellationToken: cancellationToken);
    }
}