using System.Text.Json.Serialization;

namespace FluxConfig.Storage.Api.Contracts.InternalAPI;

public record AuthClientResponse(
    [property: JsonPropertyName("config_key")]string ConfigurationKey
);