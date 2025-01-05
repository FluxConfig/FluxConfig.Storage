using System.Text.Json.Serialization;

namespace FluxConfig.Storage.Api.Contracts.InternalAPI;

public record AuthClientRequest(
    [property: JsonPropertyName("api_key")]string ApiKey
);