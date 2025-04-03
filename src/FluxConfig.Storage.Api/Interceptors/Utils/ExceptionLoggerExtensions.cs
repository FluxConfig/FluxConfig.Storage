using System.Text;
using FluentValidation;
using FluentValidation.Results;
using FluxConfig.Storage.Api.Extensions;
using FluxConfig.Storage.Domain.Exceptions.Domain;
using FluxConfig.Storage.Domain.Exceptions.Infrastructure;
using FluxConfig.Storage.Infrastructure.ISC.Exceptions;

namespace FluxConfig.Storage.Api.Interceptors.Utils;

internal static class ExceptionLoggerExtensions
{
    internal static void LogApplicationException(this ILogger logger, Exception exception, string callId,
        DateTime curDateTime)
    {
        switch (exception)
        {
            case DomainValidationException ex:
                ValidationException? innerValidationException = (ValidationException?)ex.InnerException;

                logger.LogDomainBadRequestError(
                    callId: callId,
                    curTime: curDateTime,
                    violations: QueryUnvalidatedFields(innerValidationException)
                );

                break;

            case DomainNotFoundException ex:

                EntityNotFoundException? innerNotFoundException = (EntityNotFoundException?)ex.InnerException;

                logger.LogDomainNotFoundError(
                    callId: callId,
                    curTime: curDateTime,
                    key: innerNotFoundException?.ConfigurationKey,
                    tag: innerNotFoundException?.ConfigurationTag
                );

                break;

            case DomainAlreadyExistsException ex:

                EntityAlreadyExistsException? innerAExistsException = (EntityAlreadyExistsException?)ex.InnerException;

                logger.LogDomainDuplicateConfigError(
                    callId: callId,
                    curTime: curDateTime,
                    key: innerAExistsException?.ConfigurationKey,
                    tag: innerAExistsException?.ConfigurationKey
                );

                break;

            case ClientServiceUnauthenticatedException ex:

                logger.LogClientServiceAuthError(
                    callId: callId,
                    curTime: curDateTime,
                    apiKey: ex.ApiKey
                );

                break;

            case InternalServiceUnauthenticatedException ex:
                
                if (ex.IsOutgoing)
                {
                    logger.LogInternalServiceOutgoingAuthError(
                        callId: callId,
                        curTime: curDateTime,
                        apiKey: ex.ApiKey
                    );
                }
                else
                {
                    logger.LogInternalServiceIncomingAuthError(
                        callId: callId,
                        curTime: curDateTime,
                        apiKey: ex.ApiKey
                    );
                }

                break;


            case FcManagementUnavailableException ex:
                logger.LogFcManagementUnavailable(
                    callId: callId,
                    curTime: curDateTime,
                    address: ex.Address
                );

                break;

            case FcManagementResponseException ex:
                logger.LogFcManagementUnexpectedResponse(
                    callId: callId,
                    curTime: curDateTime,
                    address: ex.Address,
                    statusCode: (int)ex.StatusCode,
                    namedStatusCode: ex.StatusCode
                );

                break;

            default:
                logger.LogInternalError(
                    exception: exception,
                    callId: callId,
                    curTime: curDateTime
                );
                break;
        }
    }

    private static string QueryUnvalidatedFields(
        ValidationException? exception)
    {
        StringBuilder validationExceptionsBuilder = new StringBuilder();

        if (exception == null)
        {
            return string.Empty;
        }

        foreach (ValidationFailure failure in exception.Errors)
        {
            validationExceptionsBuilder.Append($">> Field: {failure.PropertyName}, Error: {failure.ErrorMessage}\n");
        }

        return validationExceptionsBuilder.ToString();
    }
}