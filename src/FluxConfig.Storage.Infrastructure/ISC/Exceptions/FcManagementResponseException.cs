using System.Net;

namespace FluxConfig.Storage.Infrastructure.ISC.Exceptions;

public class FcManagementResponseException: Exception
{
    public HttpStatusCode StatusCode { get; init; }
    public Uri? Address { get; init; }
    
    public FcManagementResponseException(string? message, HttpStatusCode code, Uri? address) : base(message)
    {
        StatusCode = code;
        Address = address;
    }
}