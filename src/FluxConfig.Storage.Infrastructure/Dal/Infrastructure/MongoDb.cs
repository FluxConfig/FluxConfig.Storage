using FluxConfig.Storage.Domain.Contracts.Dal.Entities;
using FluxConfig.Storage.Infrastructure.Configuration.Models;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;

namespace FluxConfig.Storage.Infrastructure.Dal.Infrastructure;

public static class MongoDb
{
    public static void AddClient(IServiceCollection services, MongoDbConnectionOptions mongoOptions)
    {
        services.AddSingleton<IMongoClient>(new MongoClient(mongoOptions.GenerateMongoClientSettings()));
    }

    public static void MapComplexTypes()
    {
        BsonClassMap.RegisterClassMap<ConfigurationDataEntity>(classMap =>
            {
                classMap.AutoMap();
                classMap.MapIdMember(e => e.Id).SetIdGenerator(ObjectIdGenerator.Instance);
                classMap.MapMember(e => e.ApiKey).SetElementName("api_key");
                classMap.MapMember(e => e.ConfigurationTag).SetElementName("config_tag");
                classMap.MapMember(e => e.ConfigurationData).SetElementName("config_data").SetDefaultValue(new BsonDocument());
            }
        );
    }

    private static MongoClientSettings GenerateMongoClientSettings(this MongoDbConnectionOptions connectionOptions)
    {
        var settings = MongoClientSettings.FromConnectionString(connectionOptions.ConnectionString);
        settings.Credential = MongoCredential.CreateCredential(
            databaseName: connectionOptions.AuthDb,
            username: connectionOptions.DbUsername,
            password: connectionOptions.DbPassword
        );

        return settings;
    }
}