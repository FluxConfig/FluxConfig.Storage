namespace FluxConfig.Storage.Infrastructure.ISC.Exceptions;

public class ServiceUnauthenticatedException: Exception
{
    public ServiceUnauthenticatedException()
    {
    }

    public ServiceUnauthenticatedException(string? message) : base(message)
    {
    }

    public ServiceUnauthenticatedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}