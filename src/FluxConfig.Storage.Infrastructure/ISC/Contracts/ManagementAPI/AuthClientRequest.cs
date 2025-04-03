using System.Text.Json.Serialization;

namespace FluxConfig.Storage.Infrastructure.ISC.Contracts.ManagementAPI;

public record AuthClientRequest(
    [property: JsonPropertyName("api_key")]string ApiKey
);