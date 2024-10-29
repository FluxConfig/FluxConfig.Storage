namespace FluxConfig.Storage.Domain.Exceptions.Infrastructure;

public class EntityNotFoundException: InfrastructureException
{
    public EntityNotFoundException() {}
    
    public EntityNotFoundException(string? message): base(message) {}
    
    public EntityNotFoundException(string? message, Exception? exception): base(message, exception) {}
}