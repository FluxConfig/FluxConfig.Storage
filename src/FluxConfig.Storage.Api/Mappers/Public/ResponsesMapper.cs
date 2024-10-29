using FluxConfig.Storage.Api.GrpcContracts.Public;
using FluxConfig.Storage.Domain.Models.Public;

namespace FluxConfig.Storage.Api.Mappers.Public;

public static class ResponsesMapper
{
    public static LoadConfigResponse MapModelToResponse(this ConfigurationDataModel model)
    {
        var response = new LoadConfigResponse();
        response.ConfigurationData.Add(model.ConfigurationData);
        return response;
    }
}