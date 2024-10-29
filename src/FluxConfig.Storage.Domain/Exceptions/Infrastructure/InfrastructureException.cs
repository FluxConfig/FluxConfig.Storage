namespace FluxConfig.Storage.Domain.Exceptions.Infrastructure;

public class InfrastructureException: Exception
{
    public InfrastructureException() {}
    
    public InfrastructureException(string? message): base(message) {}
    
    public InfrastructureException(string? message, Exception? exception): base(message, exception) {}
    
}