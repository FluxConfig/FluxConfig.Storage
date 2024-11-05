namespace FluxConfig.Storage.Domain.Exceptions.Infrastructure;

public class EntityNotFoundException: InfrastructureException
{
    public string ConfigurationTag { get; init; } = string.Empty;

    public EntityNotFoundException()
    {
    }

    public EntityNotFoundException(string? message, string tag) : base(message)
    {
        ConfigurationTag = tag;
    }

    public EntityNotFoundException(string? message, string tag, Exception? exception) : base(message, exception)
    {
        ConfigurationTag = tag;
    }
}