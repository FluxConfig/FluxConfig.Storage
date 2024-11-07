using MongoDB.Bson;

namespace FluxConfig.Storage.Domain.Contracts.Dal.Containers;

public record UpdateConfigurationContainer(
    string ConfigurationKey,
    string ConfigurationTag,
    BsonDocument ConfigurationData
)
{
};