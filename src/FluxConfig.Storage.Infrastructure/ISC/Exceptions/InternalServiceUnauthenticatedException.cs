namespace FluxConfig.Storage.Infrastructure.ISC.Exceptions;

public class InternalServiceUnauthenticatedException: Exception
{
    public bool IsOutgoing { get; init; }
    public string ApiKey { get; init; }
    public InternalServiceUnauthenticatedException(string? message, string apiKey, bool outgoing) : base(message)
    {
        IsOutgoing = outgoing;
        ApiKey = apiKey;
    }

}