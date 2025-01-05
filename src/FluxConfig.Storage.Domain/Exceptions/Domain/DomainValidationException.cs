using FluentValidation;

namespace FluxConfig.Storage.Domain.Exceptions.Domain;

public class DomainValidationException : DomainException
{
    public DomainValidationException(string? message, ValidationException? innerException)
        : base(message, innerException)
    {
    }
}