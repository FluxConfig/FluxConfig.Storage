using FluxConfig.Storage.Infrastructure.ISC.Contracts.ManagementAPI;

namespace FluxConfig.Storage.Infrastructure.ISC.Clients.Interfaces;

public interface IManagementServiceClient
{
    public Task<AuthClientResponse> AuthenticateClientService(AuthClientRequest request, CancellationToken cancellationToken);
}