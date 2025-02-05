using FluxConfig.Storage.Api.Contracts.InternalAPI;

namespace FluxConfig.Storage.Api.Clients.Interfaces;

public interface IManagementServiceClient
{
    public Task<AuthClientResponse> AuthenticateClient(AuthClientRequest request, CancellationToken cancellationToken);
}