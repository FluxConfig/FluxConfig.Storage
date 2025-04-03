namespace FluxConfig.Storage.Domain.Exceptions.Infrastructure;

public class EntityAlreadyExistsException: InfrastructureException
{
    public string ConfigurationTag { get; init; } = string.Empty;
    public string ConfigurationKey { get; init; } = string.Empty;

    public EntityAlreadyExistsException()
    {
    }

    public EntityAlreadyExistsException(string? message, string key, string tag) : base(message)
    {
        ConfigurationKey = key;
        ConfigurationTag = tag;
    }

    public EntityAlreadyExistsException(string? message, string key, string tag, Exception? exception) : base(message, exception)
    {
        ConfigurationKey = key;
        ConfigurationTag = tag;
    }
}