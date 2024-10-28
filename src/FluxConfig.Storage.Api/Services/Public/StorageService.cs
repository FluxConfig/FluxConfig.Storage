using FluxConfig.Storage.Api.GrpcContracts.Public;
using Grpc.Core;

namespace FluxConfig.Storage.Api.Services.Public;

public class StoragePublicService : GrpcContracts.Public.Storage.StorageBase
{
    private readonly ILogger<StoragePublicService> _logger;

    public StoragePublicService(ILogger<StoragePublicService> logger)
    {
        _logger = logger;
    }

    public override async Task<LoadConfigResponse> LoadVaultConfig(LoadConfigRequest request, ServerCallContext context)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(5));
        throw new NotImplementedException();
    }

    public override async Task<LoadConfigResponse> LoadRealTimeConfig(LoadConfigRequest request,
        ServerCallContext context)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(5));
        throw new NotImplementedException();
    }
}