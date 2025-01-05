namespace FluxConfig.Storage.Domain.Exceptions.Infrastructure;

public class EntityNotFoundException: InfrastructureException
{
    public string ConfigurationTag { get; init; } = string.Empty;

    public string ConfigurationKey { get; init; } = string.Empty;

    public EntityNotFoundException()
    {
    }

    public EntityNotFoundException(string? message, string tag, string key) : base(message)
    {
        ConfigurationTag = tag;
        ConfigurationKey = key;
    }

    public EntityNotFoundException(string? message, string tag, string key, Exception? exception) : base(message, exception)
    {
        ConfigurationTag = tag;
        ConfigurationKey = key;
    }
}