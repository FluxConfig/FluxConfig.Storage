using FluxConfig.Storage.Domain.Contracts.Dal.Entities;
using MongoDB.Bson;

namespace FluxConfig.Storage.Domain.Models.Internal.Mappers;

public static class EntityMappers
{
    public static ConfigurationDataModel MapEntityToModel(this ConfigurationDataEntity entity)
    {
        return new ConfigurationDataModel(
            RawJsonConfigurationData: entity.ConfigurationData.ToJson()
        );
    }
}