using MongoDB.Bson;

namespace FluxConfig.Storage.Domain.Contracts.Dal.Entities;

public class ConfigurationDataEntity
{
    public ObjectId Id { get; set; }
    public string ApiKey { get; set; } = string.Empty;
    public string ConfigurationTag { get; set; } = string.Empty;
    public BsonDocument ConfigurationData { get; set; } = [];
}