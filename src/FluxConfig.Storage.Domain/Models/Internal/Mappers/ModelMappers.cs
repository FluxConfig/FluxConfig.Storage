using FluxConfig.Storage.Domain.Contracts.Dal.Entities;

namespace FluxConfig.Storage.Domain.Models.Internal.Mappers;

public static class ModelMappers
{
    public static ConfigurationDataEntity MapModelToEntity(this CreateConfigurationModel model)
    {
        return new ConfigurationDataEntity()
        {
            ConfigurationKey = model.ConfigurationKey,
            ConfigurationTag = model.ConfigurationTag,
        };
    }
}