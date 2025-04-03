using System.Text;
using FluxConfig.Storage.Api.Extensions;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace FluxConfig.Storage.Api.Interceptors;

public class HeadersLoggerInterceptor : Interceptor
{
    private readonly ILogger<HeadersLoggerInterceptor> _logger;

    public HeadersLoggerInterceptor(ILogger<HeadersLoggerInterceptor> logger)
    {
        _logger = logger;
    }
    
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {   
        LogHeaders(context);

        return await continuation(request, context);
    }
    
    private void LogHeaders(ServerCallContext context)
    {
        StringBuilder headersMetadataBuilder = new StringBuilder();

        using IEnumerator<Metadata.Entry> headersEnumerator = context.RequestHeaders.GetEnumerator();
        while (headersEnumerator.MoveNext())
        {
            headersMetadataBuilder.Append($">>header: {headersEnumerator.Current.Key}, value: {headersEnumerator.Current.Value}\n");
        }
        headersEnumerator.Reset();;

        string callId = string.Empty;
        if (context.UserState.TryGetValue(LoggerInterceptor.CallIdKey, out var value))
        {
            callId = (value as string) ?? "";;
        }
        
        _logger.LogCallHeadersMeta(callId, DateTime.Now, headersMetadataBuilder.ToString());
    }
}