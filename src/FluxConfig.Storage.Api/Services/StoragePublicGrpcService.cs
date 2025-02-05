using FluxConfig.Storage.Api.GrpcContracts.Public;
using FluxConfig.Storage.Api.Mappers.Public;
using FluxConfig.Storage.Domain.Services.Interfaces;
using Grpc.Core;

namespace FluxConfig.Storage.Api.Services;

public class StoragePublicGrpcService : GrpcContracts.Public.Storage.StorageBase
{
    private readonly IStoragePublicService _storagePublicService;

    public StoragePublicGrpcService(IStoragePublicService storageService)
    {
        _storagePublicService = storageService;
    }

    public override async Task<LoadConfigResponse> LoadVaultConfig(LoadConfigRequest request, ServerCallContext context)
    {
        var loadVaultConfigModel = request.MapRequestToModel(
            callContext: context
            );

        var configDataModel = await _storagePublicService.GetVaultConfigurationData(
            loadConfigModel: loadVaultConfigModel,
            cancellationToken: context.CancellationToken);

        return configDataModel.MapModelToResponse();
    }

    public override async Task<LoadConfigResponse> LoadRealTimeConfig(LoadConfigRequest request,
        ServerCallContext context)
    {
        var loadRealTimeConfigModel = request.MapRequestToModel(
            callContext: context
            );

        var configDataModel = await _storagePublicService.GetRealTimeConfigurationData(
            loadConfigModel: loadRealTimeConfigModel,
            cancellationToken: context.CancellationToken);

        return configDataModel.MapModelToResponse();
    }
}