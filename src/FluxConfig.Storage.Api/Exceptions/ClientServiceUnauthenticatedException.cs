namespace FluxConfig.Storage.Api.Exceptions;

public class ClientServiceUnauthenticatedException: Exception
{
    public ClientServiceUnauthenticatedException()
    {
    }

    public ClientServiceUnauthenticatedException(string? message) : base(message)
    {
    }

    public ClientServiceUnauthenticatedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}