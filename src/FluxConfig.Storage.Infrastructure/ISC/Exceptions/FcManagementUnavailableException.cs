namespace FluxConfig.Storage.Infrastructure.ISC.Exceptions;

public class FcManagementUnavailableException: Exception
{
    public Uri? Address { get; init; }
    
    public FcManagementUnavailableException(string? message, Uri? address) : base(message)
    {
        Address = address;
    }

    public FcManagementUnavailableException(string? message, Uri? address, Exception? innerException) : base(message, innerException)
    {
        Address = address;
    }
}