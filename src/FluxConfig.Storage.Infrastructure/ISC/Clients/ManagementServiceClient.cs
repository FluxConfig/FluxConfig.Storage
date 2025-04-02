using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using FluxConfig.Storage.Infrastructure.ISC.Clients.Interfaces;
using FluxConfig.Storage.Infrastructure.ISC.Contracts.ManagementAPI;
using FluxConfig.Storage.Infrastructure.ISC.Exceptions;
using Microsoft.Extensions.Logging;

namespace FluxConfig.Storage.Infrastructure.ISC.Clients;

public class ManagementServiceClient : IManagementServiceClient
{
    internal const string ManagementClientTag = "FCManagement";
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ManagementServiceClient> _logger;

    public ManagementServiceClient(ILogger<ManagementServiceClient> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
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
                requestUri: $"/api/fcm/configurations/client-service/auth?api_key={request.ApiKey}",
                cancellationToken: cancellationToken
            );
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "[{curDate}] Unable to get the response from auth service from address {address}",
                DateTime.Now, client.BaseAddress);
            throw new AuthServiceException("Unable to get the response from auth service", ex);
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            var exc = new ServiceUnauthenticatedException(
                "Invalid internal api-key metadata, needed to authenticate request to authentication api.");
            _logger.LogError(exc,
                "[{curDate}] Invalid internal api-key metadata, needed to authenticate request to authentication api.",
                DateTime.Now);
            throw exc;
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            var exc = new ClientServiceUnauthenticatedException("Invalid x-api-key authentication metadata.");
            _logger.LogError(exc, "[{curDate}] Invalid x-api-key authentication metadata: X-API-KEY: {key}.",
                DateTime.Now, request.ApiKey);
            throw exc;
        }

        if (!response.IsSuccessStatusCode)
        {
            var exc = new AuthServiceException(
                $"[{DateTime.Now}] Unable to get the response from auth service with status-code: {(int)response.StatusCode} - {response.StatusCode}");
            _logger.LogError(exc,
                "[{curTime}] Unable to get the response from auth service with status-code: {numCode} - {namedCode}",
                DateTime.Now, (int)response.StatusCode, response.StatusCode);
            throw exc;
        }

        var rawJson = await response.Content.ReadAsStringAsync(cancellationToken);
        var authResponse = JsonSerializer.Deserialize<AuthClientResponse>(rawJson) ??
                           throw new JsonException("Invalid auth service response format.");

        return authResponse;
    }
}