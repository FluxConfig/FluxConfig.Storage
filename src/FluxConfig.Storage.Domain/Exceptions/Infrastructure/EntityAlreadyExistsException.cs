namespace FluxConfig.Storage.Domain.Exceptions.Infrastructure;

public class EntityAlreadyExistsException: InfrastructureException
{
    public string ConfigurationTag { get; init; } = string.Empty;

    public EntityAlreadyExistsException()
    {
    }

    public EntityAlreadyExistsException(string? message, string tag) : base(message)
    {
        ConfigurationTag = tag;
    }

    public EntityAlreadyExistsException(string? message, string tag, Exception? exception) : base(message, exception)
    {
        ConfigurationTag = tag;
    }
}