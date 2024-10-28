using Grpc.Core;
using FluxConfig.Storage.Api.GrpcContracts.Internal;

namespace FluxConfig.Storage.Api.Services.Internal;

public class StorageInternalService : GrpcContracts.Internal.Storage.StorageBase
{
    private readonly ILogger<StorageInternalService> _logger;

    public StorageInternalService(ILogger<StorageInternalService> logger)
    {
        _logger = logger;
    }

    public override async Task<LoadConfigResponse> LoadVaultConfig(LoadConfigRequest request, ServerCallContext context)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(5));
        throw new NotImplementedException();
    }

    public override async Task<LoadConfigResponse> LoadRealTimeConfig(LoadConfigRequest request, ServerCallContext context)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(5));
        throw new NotImplementedException();
    }

    public override async Task<UpdateConfigResponse> UpdateVaultConfig(UpdateConfigRequest request, ServerCallContext context)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(5));
        throw new NotImplementedException();
    }

    public override async Task<UpdateConfigResponse> UpdateRTConfig(UpdateConfigRequest request, ServerCallContext context)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(5));
        throw new NotImplementedException();
    }

    public override async Task<CreateServiceConfigResponse> CreateServiceConfiguration(CreateServiceConfigRequest request, ServerCallContext context)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(5));
        throw new NotImplementedException();
    }

    public override async Task<DeleteServiceConfigResponse> DeleteServiceConfiguration(DeleteServiceConfigRequest request, ServerCallContext context)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(5));
        throw new NotImplementedException();
    }
}