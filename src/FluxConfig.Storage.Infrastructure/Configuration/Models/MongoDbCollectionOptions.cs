namespace FluxConfig.Storage.Infrastructure.Configuration.Models;

public class MongoDbCollectionOptions
{
    public const string VaultTag = "Vault";
    public const string RealTimeTag = "RealTime";
    
    public string DatabaseName { get; init; } = string.Empty;
    public string CollectionName { get; init; } = string.Empty;
}