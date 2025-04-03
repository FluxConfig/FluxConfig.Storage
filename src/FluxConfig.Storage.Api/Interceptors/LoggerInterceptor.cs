using FluxConfig.Storage.Api.Extensions;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace FluxConfig.Storage.Api.Interceptors;

public class LoggerInterceptor : Interceptor
{
    private readonly ILogger<LoggerInterceptor> _logger;
    internal static readonly object CallIdKey = new();

    public LoggerInterceptor(ILogger<LoggerInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        string callId = Guid.NewGuid().ToString();
        context.UserState[CallIdKey] = callId;

        using (_logger.BeginScope(new Dictionary<string, object> { ["CallId"] = callId }))
        {
            _logger.LogCallStart(callId, DateTime.Now, context.Method);

            var response = await continuation(request, context);

            _logger.LogSuccessCallEnd(callId, DateTime.Now, context.Method);

            return response;
        }
    }
}