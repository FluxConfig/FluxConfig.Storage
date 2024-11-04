using FluxConfig.Storage.Domain.Contracts.Dal.Entities;
using FluxConfig.Storage.Domain.Contracts.Dal.Interfaces;
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

    public async Task<ConfigurationDataEntity> LoadConfiguration(string serviceApiKey, string configurationTag,
        CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMicroseconds(5), cancellationToken);
        throw new NotImplementedException();
    }
}