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
                classMap.MapMember(e => e.ConfigurationKey).SetElementName("key");
                classMap.MapMember(e => e.ConfigurationTag).SetElementName("tag");
                classMap.MapMember(e => e.ConfigurationData).SetElementName("data").SetDefaultValue(new BsonDocument());
            }
        );
    }

    private static MongoClientSettings GenerateMongoClientSettings(this MongoDbConnectionOptions connectionOptions)
    {
        var settings = MongoClientSettings.FromConnectionString(connectionOptions.ConnectionString);
        settings.Credential = MongoCredential.CreateCredential(
            databaseName: connectionOptions.AuthDb,
            username: Environment.GetEnvironmentVariable("MONGO_USERNAME") ??
                      throw new ArgumentException("MongoDB username is missing."),
            password: Environment.GetEnvironmentVariable("MONGO_PASSWORD") ??
                      throw new ArgumentException("MongoDB password is missing.")
        );
        settings.ApplicationName = connectionOptions.ApplicationName;

        return settings;
    }
}