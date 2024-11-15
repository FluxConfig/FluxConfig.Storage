using FluxConfig.Storage.Domain.Contracts.Dal.Containers;
using MongoDB.Bson;

namespace FluxConfig.Storage.Domain.Models.Internal.Mappers;

public static class ContainersMapper
{
    public static UpdateConfigurationContainer MapModelToContainer(this UpdateConfigurationModel model)
    {
        return new UpdateConfigurationContainer(
            ConfigurationKey: model.ConfigurationKey,
            ConfigurationTag: model.ConfigurationTag,
            ConfigurationData: BsonDocument.Parse(model.RawJsonConfigurationData)
        );
    }

    public static ChangeTagContainer MapModelToContainer(this ChangeConfigTagModel model)
    {
        return new ChangeTagContainer(
            ConfigurationKey: model.ConfigurationKey,
            OldConfigTag: model.OldConfigurationTag,
            NewConfigTag: model.NewConfigurationTag
        );
    }
}