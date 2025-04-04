using FluxConfig.Storage.Api.Interceptors.Utils;
using FluxConfig.Storage.Domain.Exceptions.Domain;
using FluxConfig.Storage.Infrastructure.ISC.Exceptions;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Status = Google.Rpc.Status;

namespace FluxConfig.Storage.Api.Interceptors.Public;

public class ExceptionHandlerInterceptor: Interceptor
{
    private readonly ILogger<ExceptionHandlerInterceptor> _logger;

    public ExceptionHandlerInterceptor(ILogger<ExceptionHandlerInterceptor> logger)
    {
        _logger = logger;
    }
    
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
            string callId = string.Empty;
            if (context.UserState.TryGetValue(LoggerInterceptor.CallIdKey, out var value))
            {
                callId = (value as string) ?? "";
            }
            
            _logger.LogApplicationException(ex, callId, DateTime.Now);
            RpcException rpcEx = MapExceptionToRpcException(ex, context);
            throw rpcEx;
        }
    }

    private static RpcException MapExceptionToRpcException(Exception ex, ServerCallContext context)
    {
        Status status = ex switch
        {   
            DomainValidationException exception => RpcExceptionGenerator.GenerateBadRequestException(exception),
            
            DomainNotFoundException exception => RpcExceptionGenerator.PublicGenerateNotFoundException(exception, context),
            
            ClientServiceUnauthenticatedException => RpcExceptionGenerator.GenerateUnauthenticatedException(context),
            
            _ => RpcExceptionGenerator.GenerateInternalException(
                callContext: context)
        };

        return status.ToRpcException();
    }
}