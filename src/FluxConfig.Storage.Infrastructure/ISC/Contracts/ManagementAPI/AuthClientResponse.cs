using System.Text.Json.Serialization;

namespace FluxConfig.Storage.Infrastructure.ISC.Contracts.ManagementAPI;

public record AuthClientResponse(
    [property: JsonPropertyName("config_key")]string ConfigurationKey
);