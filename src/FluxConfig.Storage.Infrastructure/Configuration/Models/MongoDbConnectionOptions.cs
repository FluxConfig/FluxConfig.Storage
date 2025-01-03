namespace FluxConfig.Storage.Infrastructure.Configuration.Models;

public class MongoDbConnectionOptions
{
    public string ConnectionString { get; init; } = string.Empty;
    public string AuthDb { get; init; } = string.Empty;
    public string ApplicationName { get; init; } = string.Empty;
}