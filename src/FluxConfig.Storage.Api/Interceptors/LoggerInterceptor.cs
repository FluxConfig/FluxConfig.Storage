using Grpc.Core;
using Grpc.Core.Interceptors;

namespace FluxConfig.Storage.Api.Interceptors;

public class LoggerInterceptor : Interceptor
{
    private readonly ILogger<LoggerInterceptor> _logger;

    public LoggerInterceptor(ILogger<LoggerInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {   
        LogMethodCall<TRequest, TResponse>(context);

        return await continuation(request, context);
    }

    private void LogMethodCall<TRequest, TResponse>(ServerCallContext context)
        where TRequest : class
        where TResponse : class
    {
        _logger.LogInformation("[{curTime}] Start executing call. Method: {methodName}, Request: {requestType}, Response: {responseType}", DateTime.Now,
            context.Method, typeof(TRequest), typeof(TResponse));
    }
}