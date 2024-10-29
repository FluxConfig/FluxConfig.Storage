using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Google.Rpc;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Status = Google.Rpc.Status;

namespace FluxConfig.Storage.Api.Interceptors;

public class ExceptionHandlerInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (Exception ex)
        {
            RpcException newEx = MapExceptionToRpcException(ex, context);
            throw newEx;
        }
    }

    private static RpcException MapExceptionToRpcException(Exception ex, ServerCallContext context)
    {
        Status status = ex switch
        {
            NotImplementedException => GenerateNotImplementedException(
                exception: ex,
                callContext: context),

            _ => GenerateInternalException(
                exception: ex,
                callContext: context)
        };

        return status.ToRpcException();
    }

    private static Status GenerateNotImplementedException(Exception exception, ServerCallContext callContext)
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

    private static Status GenerateInternalException(Exception exception, ServerCallContext callContext)
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