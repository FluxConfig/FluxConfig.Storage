using FluxConfig.Storage.Domain.Exceptions.Infrastructure;

namespace FluxConfig.Storage.Domain.Exceptions.Domain;

public class DomainNotFoundException : DomainException
{
    public DomainNotFoundException(string? message, EntityNotFoundException? innerException) :
        base(message, innerException)
    {
    }
}