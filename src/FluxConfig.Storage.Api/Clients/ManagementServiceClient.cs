using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using FluxConfig.Storage.Api.Clients.Interfaces;
using FluxConfig.Storage.Api.Contracts.InternalAPI;
using FluxConfig.Storage.Api.Exceptions;

namespace FluxConfig.Storage.Api.Clients;

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

    public async Task<AuthClientResponse> AuthenticateClient(AuthClientRequest request,
        CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient(ManagementClientTag);

        var jsonRequest = new StringContent(
            content: JsonSerializer.Serialize(request),
            encoding: Encoding.UTF8,
            mediaType: MediaTypeNames.Application.Json
        );

        HttpResponseMessage response;

        try
        {
            response = await client.PostAsync(
                requestUri: "/management/services/auth",
                content: jsonRequest,
                cancellationToken: cancellationToken
            );
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Unable to get the response from auth service");
            throw new AuthServiceException("Unable to get the response from auth service", ex);
        }
        
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            var exc = new ServiceUnauthenticatedException("Invalid internal api-key metadata, needed to authenticate request to authentication api.");
            _logger.LogError(exc, "Invalid internal api-key metadata, needed to authenticate request to authentication api.");
            throw exc;
        }
        
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var exc = new ClientServiceUnauthenticatedException("Invalid x-api-key authentication metadata.");
            _logger.LogError(exc, "Invalid x-api-key authentication metadata.");
            throw exc;
        }

        if (!response.IsSuccessStatusCode)
        {
            string exceptionString = $"Unable to get the response from auth service with status-code: {(int)response.StatusCode} - {response.StatusCode}";
            var exc =  new AuthServiceException(exceptionString);
            _logger.LogError(exc, exceptionString);
            throw exc;
        }

        var rawJson = await response.Content.ReadAsStringAsync(cancellationToken);
        var authResponse = JsonSerializer.Deserialize<AuthClientResponse>(rawJson) ??
                           throw new JsonException("Invalid auth service response format.");

        return authResponse;
    }
}