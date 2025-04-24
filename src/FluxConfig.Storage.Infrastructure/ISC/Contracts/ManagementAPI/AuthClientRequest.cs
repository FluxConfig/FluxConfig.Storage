using System.Text.Json.Serialization;

namespace FluxConfig.Storage.Infrastructure.ISC.Contracts.ManagementAPI;

public record AuthClientRequest(
    string ApiKey,
    string Tag
);