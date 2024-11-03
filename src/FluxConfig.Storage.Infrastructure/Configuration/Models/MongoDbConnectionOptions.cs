namespace FluxConfig.Storage.Infrastructure.Configuration.Models;

public class MongoDbConnectionOptions
{
    public string ConnectionString { get; init; } = string.Empty;
    public string DbUsername { get; init; } = string.Empty;
    public string DbPassword { get; init; } = string.Empty;
    public string AuthDb { get; init; } = string.Empty;
    public string AuthMechanism { get; init; } = string.Empty;
}