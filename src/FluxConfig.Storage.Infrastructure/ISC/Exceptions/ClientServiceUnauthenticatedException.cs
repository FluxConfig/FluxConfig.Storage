namespace FluxConfig.Storage.Infrastructure.ISC.Exceptions;

public class ClientServiceUnauthenticatedException: Exception
{
    public string ApiKey { get; set; }

    public ClientServiceUnauthenticatedException(string? message, string apiKey) : base(message)
    {
        ApiKey = apiKey;
    }

}