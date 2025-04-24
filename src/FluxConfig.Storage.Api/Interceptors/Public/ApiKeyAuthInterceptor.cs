using FluxConfig.Storage.Api.GrpcContracts.Public;
using FluxConfig.Storage.Infrastructure.ISC.Clients.Interfaces;
using FluxConfig.Storage.Infrastructure.ISC.Contracts.ManagementAPI;
using FluxConfig.Storage.Infrastructure.ISC.Exceptions;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace FluxConfig.Storage.Api.Interceptors.Public;

public class ApiKeyAuthInterceptor : Interceptor
{
    private readonly IManagementServiceClient _managementServiceClient;

    public ApiKeyAuthInterceptor(IManagementServiceClient client)
    {
        _managementServiceClient = client;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        string configurationTag;
        if (request is LoadConfigRequest loadConfigRequest)
        {
            configurationTag = loadConfigRequest.ConfigurationTag ?? "";
        }
        else
        {
            throw new ClientServiceUnauthenticatedException("Invalid rpc method message type", "");
        }
        
        await Authenticate(context, configurationTag);
        return await continuation(request, context);
    }

    private async Task Authenticate(ServerCallContext context, string configurationTag)
    {
        string apiKey = context.RequestHeaders.GetValue("X-API-KEY") ??
                        throw new ClientServiceUnauthenticatedException("Empty x-api-key authentication metadata.", "");
        
        var authResponse = await _managementServiceClient.AuthenticateClientService(
            request: new AuthClientRequest(
                ApiKey: apiKey,
                Tag: configurationTag
            ),
            cancellationToken: context.CancellationToken
        );

        context.RequestHeaders.Add("X-CFG-KEY", authResponse.ConfigurationKey);
    }
}