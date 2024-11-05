using FluxConfig.Storage.Api.Interceptors.Utils;
using FluxConfig.Storage.Domain.Exceptions.Domain;
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
            DomainValidationException exception => RpcExceptionGenerator.GenerateBadRequestException(exception),
            
            DomainNotFoundException exception => RpcExceptionGenerator.GenerateNotFoundException(exception, context),

            NotImplementedException => RpcExceptionGenerator.GenerateNotImplementedException(
                callContext: context),
            
            _ => RpcExceptionGenerator.GenerateInternalException(
                callContext: context)
        };

        return status.ToRpcException();
    }
}