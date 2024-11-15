using FluxConfig.Storage.Api.GrpcContracts.Internal;
using FluxConfig.Storage.Api.Mappers.Internal;
using FluxConfig.Storage.Domain.Models.Internal;
using FluxConfig.Storage.Domain.Services.Interfaces;
using Grpc.Core;

namespace FluxConfig.Storage.Api.Services;

public class StorageInternalGrpcService : GrpcContracts.Internal.Storage.StorageBase
{
    private readonly IStorageInternalService _storageService;

    public StorageInternalGrpcService(IStorageInternalService storageInternalService)
    {
        _storageService = storageInternalService;
    }

    public override async Task<LoadConfigResponse> LoadVaultConfig(LoadConfigRequest request, ServerCallContext context)
    {
        LoadConfigurationModel loadModel = request.MapRequestToModel();

        var configModel = await _storageService.GetVaultConfigurationData(
            model: loadModel,
            cancellationToken: context.CancellationToken
        );

        return configModel.MapModelToResponse();
    }

    public override async Task<LoadConfigResponse> LoadRealTimeConfig(LoadConfigRequest request,
        ServerCallContext context)
    {
        LoadConfigurationModel loadModel = request.MapRequestToModel();

        var configModel = await _storageService.GetRealTimeConfigurationData(
            model: loadModel,
            cancellationToken: context.CancellationToken
        );

        return configModel.MapModelToResponse();
    }

    public override async Task<UpdateConfigResponse> UpdateVaultConfig(UpdateConfigRequest request,
        ServerCallContext context)
    {
        UpdateConfigurationModel updateModel = request.MapRequestToModel();

        await _storageService.UpdateVaultConfiguration(
            model: updateModel,
            cancellationToken: context.CancellationToken
        );

        return new UpdateConfigResponse();
    }

    public override async Task<UpdateConfigResponse> UpdateRTConfig(UpdateConfigRequest request,
        ServerCallContext context)
    {
        UpdateConfigurationModel updateModel = request.MapRequestToModel();

        await _storageService.UpdateRealTimeConfiguration(
            model: updateModel,
            cancellationToken: context.CancellationToken
        );

        return new UpdateConfigResponse();
    }

    public override async Task<CreateServiceConfigResponse> CreateServiceConfiguration(
        CreateServiceConfigRequest request, ServerCallContext context)
    {
        CreateConfigurationModel createModel = request.MapRequestToModel();

        await _storageService.CreateNewServiceConfiguration(
            model: createModel,
            cancellationToken: context.CancellationToken
        );

        return new CreateServiceConfigResponse();
    }

    public override async Task<DeleteServiceConfigResponse> DeleteServiceConfiguration(
        DeleteServiceConfigRequest request, ServerCallContext context)
    {
        DeleteConfigurationModel deleteModel = request.MapRequestToModel();

        await _storageService.DeleteServiceConfiguration(
            model: deleteModel,
            cancellationToken: context.CancellationToken
        );

        return new DeleteServiceConfigResponse();
    }

    public override async Task<ChangeConfigurationTagResponse> ChangeServiceConfigurationTag(
        ChangeConfigurationTagRequest request, ServerCallContext context)
    {
        await Task.Delay(TimeSpan.FromMicroseconds(5), context.CancellationToken);
        throw new NotImplementedException();
    }
}