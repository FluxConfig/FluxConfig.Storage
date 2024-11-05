using FluxConfig.Storage.Domain.Contracts.Dal.Entities;
using FluxConfig.Storage.Domain.Exceptions.Domain;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace FluxConfig.Storage.Domain.Models.Public.Mappers;

public static class EntityMapper
{
    public static ConfigurationDataModel MapEntityToModel(this ConfigurationDataEntity entity)
    {
        return new ConfigurationDataModel(
            ConfigurationData: ConvertBsonToDictionary(entity.ConfigurationData)
        );
    }

    private static Dictionary<string, string> ConvertBsonToDictionary(BsonDocument bsonConfigMap)
    {
        string rawJson = bsonConfigMap.ToJson();
        Dictionary<string, string> configMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(rawJson) ??
                                               throw new DomainException( "Exception occured during configuration deserialization");
        return configMap;
    }
}