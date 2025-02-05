using FluxConfig.Storage.Domain.Exceptions.Infrastructure;

namespace FluxConfig.Storage.Domain.Exceptions.Domain;

public class DomainAlreadyExistsException : DomainException
{
    public DomainAlreadyExistsException(string? message, EntityAlreadyExistsException? innerException)
        : base(message, innerException)
    {
    }
}