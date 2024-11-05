using System.Text;
using FluentValidation;
using FluentValidation.Results;
using FluxConfig.Storage.Domain.Exceptions.Domain;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Google.Rpc;
using Grpc.Core;
using Status = Google.Rpc.Status;

namespace FluxConfig.Storage.Api.Interceptors.Utils;

public static class RpcExceptionGenerator
{
    public static Status GenerateNotFoundException(DomainNotFoundException exception, ServerCallContext callContext)
    {
        return new Status
        {
            Code = (int)Code.NotFound,
            Message = "Configuration not found",
            Details =
            {
                Any.Pack(
                    new ErrorInfo
                    {
                        Reason = "NOT_FOUND",
                        Metadata =
                        {
                            new MapField<string, string>()
                            {
                                { "method", callContext.Method }
                            }
                        }
                    }
                )
            }
        };

    }

    public static Status GenerateBadRequestException(DomainValidationException exception)
    {
        return new Status
        {
            Code = (int)Code.InvalidArgument,
            Message = "Bad request.\n" + QueryExceptionMessages(exception),
            Details =
            {
                Any.Pack(new BadRequest
                {
                    FieldViolations =
                    {
                        QueryUnvalidatedFields((ValidationException?)exception.InnerException)
                    }
                })
            }
        };
    }

    private static string QueryExceptionMessages(Exception exception)
    {
        StringBuilder messages = new StringBuilder();
        messages.Append(exception.Message);

        Exception? innerException = exception.InnerException;
        while (innerException != null)
        {
            messages.Append($"\n{innerException.Message}");
            innerException = innerException.InnerException;
        }

        return messages.ToString();
    }

    private static RepeatedField<BadRequest.Types.FieldViolation> QueryUnvalidatedFields(
        ValidationException? exception)
    {
        RepeatedField<BadRequest.Types.FieldViolation> validations =
            new RepeatedField<BadRequest.Types.FieldViolation>();
        
        if (exception == null)
        {
            return validations;
        }

        foreach (ValidationFailure failure in exception.Errors)
        {
            validations.Add(new BadRequest.Types.FieldViolation
            {
                Field = failure.PropertyName,
                Description = failure.ErrorMessage
            });
        }

        return validations;
    }

    public static Status GenerateNotImplementedException(ServerCallContext callContext)
    {
        return new Status
        {
            Code = (int)Code.Unimplemented,
            Message = "Method is not implemented yet.",
            Details =
            {
                Any.Pack(
                    new ErrorInfo
                    {
                        Reason = "NOT_IMPLEMENTED",
                        Metadata =
                        {
                            new MapField<string, string>
                            {
                                { "method", callContext.Method }
                            }
                        }
                    }
                )
            }
        };
    }

    public static Status GenerateInternalException(ServerCallContext callContext)
    {
        return new Status
        {
            Code = (int)Code.Internal,
            Message = "Unknown exception occured during the method call.",
            Details =
            {
                Any.Pack(
                    new ErrorInfo
                    {
                        Reason = "INTERNAL_ERROR",
                        Metadata =
                        {
                            new MapField<string, string>()
                            {
                                { "method", callContext.Method }
                            }
                        }
                    }
                )
            }
        };
    }
}