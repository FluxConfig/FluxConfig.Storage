using System.Net;
using System.Text.Json;
using FluxConfig.Storage.Domain.Exceptions.Domain;
using FluxConfig.Storage.Domain.Exceptions.Infrastructure;
using FluxConfig.Storage.Infrastructure.ISC.Clients.Interfaces;
using FluxConfig.Storage.Infrastructure.ISC.Contracts.ManagementAPI;
using FluxConfig.Storage.Infrastructure.ISC.Exceptions;

namespace FluxConfig.Storage.Infrastructure.ISC.Clients;

public class ManagementServiceClient : IManagementServiceClient
{
    internal const string ManagementClientTag = "FCManagement";
    private readonly IHttpClientFactory _httpClientFactory;

    public ManagementServiceClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<AuthClientResponse> AuthenticateClientService(AuthClientRequest request,
        CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient(ManagementClientTag);

        HttpResponseMessage response;

        try
        {
            response = await client.GetAsync(
                requestUri: $"/api/fcm/configurations/client-service/auth?ApiKey={request.ApiKey}&Tag={request.Tag}",
                cancellationToken: cancellationToken
            );
        }
        catch (HttpRequestException ex)
        {
            throw new FcManagementUnavailableException("Unable to get the response from FC Management service",
                client.BaseAddress, ex);
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new InternalServiceUnauthenticatedException(
                "Invalid internal api-key metadata, needed to authenticate request to FC Management api.",
                apiKey: Environment.GetEnvironmentVariable("FC_API_KEY") ?? "MISSING",
                outgoing: true);
        }

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            throw new ClientServiceUnauthenticatedException("Invalid x-api-key authentication metadata.",
                request.ApiKey);
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new DomainNotFoundException(
                message: "Service configuration not found",
                innerException: new EntityNotFoundException(
                    message: "Service configuration not found",
                    tag: request.Tag,
                    key: ""
                )
            );
        }

        if (!response.IsSuccessStatusCode)
        {
            throw new FcManagementResponseException("Unexpected response from FC Management service",
                response.StatusCode, client.BaseAddress);
        }

        var rawJson = await response.Content.ReadAsStringAsync(cancellationToken);
        var authResponse = JsonSerializer.Deserialize<AuthClientResponse>(rawJson) ??
                           throw new JsonException("Unexpected FC Management service response format.");

        return authResponse;
    }
}