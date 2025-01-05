using FluentValidation;
using FluentValidation.Results;
using FluxConfig.Storage.Domain.Exceptions.Domain;
using FluxConfig.Storage.Domain.Exceptions.Infrastructure;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Google.Rpc;
using Grpc.Core;
using Status = Google.Rpc.Status;

namespace FluxConfig.Storage.Api.Interceptors.Utils;

public static class RpcExceptionGenerator
{
    public static Status PublicGenerateNotFoundException(DomainNotFoundException exception,
        ServerCallContext callContext)
    {
        return new Status
        {
            Code = (int)Code.NotFound,
            Message = "Configuration not found. Incorrect configuration tag.",
            Details =
            {
                Any.Pack(
                    new ErrorInfo
                    {
                        Reason = "INCORRECT_TAG",
                        Metadata =
                        {
                            new MapField<string, string>()
                            {
                                { "method", callContext.Method },
                                { "tag", ((EntityNotFoundException?)exception.InnerException)!.ConfigurationTag }
                            }
                        }
                    }
                )
            }
        };
    }

    public static Status InternalGenerateNotFoundException(DomainNotFoundException exception,
        ServerCallContext callContext)
    {
        return new Status
        {
            Code = (int)Code.NotFound,
            Message = "Configuration not found.",
            Details =
            {
                Any.Pack(
                    new ErrorInfo
                    {
                        Reason = "INVALID_KEY_TAG",
                        Metadata =
                        {
                            new MapField<string, string>()
                            {
                                { "method", callContext.Method },
                                { "key", ((EntityNotFoundException?)exception.InnerException)!.ConfigurationKey },
                                { "tag", ((EntityNotFoundException?)exception.InnerException)!.ConfigurationTag }
                            }
                        }
                    }
                )
            }
        };
    }

    public static Status InternalGenerateAlreadyExistsException(DomainAlreadyExistsException exception,
        ServerCallContext callContext)
    {
        return new Status
        {
            Code = (int)Code.AlreadyExists,
            Message = "Configuration already exists. Duplicate configuration tag.",
            Details =
            {
                Any.Pack(
                    new ErrorInfo
                    {
                        Reason = "DUPLICATE_TAG",
                        Metadata =
                        {
                            new MapField<string, string>()
                            {
                                { "method", callContext.Method },
                                { "tag", ((EntityAlreadyExistsException?)exception.InnerException)!.ConfigurationTag }
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
            Message = "Bad request. Invalid rpc arguments.",
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

    public static Status GenerateInternalException(ServerCallContext callContext)
    {
        return new Status
        {
            Code = (int)Code.Internal,
            Message = "Internal error.",
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
    
    public static Status GenerateUnauthenticatedException(ServerCallContext callContext)
    {
        return new Status
        {
            Code = (int)Code.Unauthenticated,
            Message = "Invalid X-API-KEY authentication metadata",
            Details =
            {
                Any.Pack(
                    new ErrorInfo
                    {
                        Reason = "INVALID_API_KEY",
                        Metadata =
                        {
                            new MapField<string, string>()
                            {   
                                { "x-api-key", callContext.RequestHeaders.GetValue("X-API-KEY") ?? ""},
                                { "method", callContext.Method }
                            }
                        }
                    }
                )
            }
        };
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
}