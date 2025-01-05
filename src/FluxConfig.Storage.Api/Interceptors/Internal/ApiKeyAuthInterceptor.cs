using FluxConfig.Storage.Api.Exceptions;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace FluxConfig.Storage.Api.Interceptors.Internal;

public class ApiKeyAuthInterceptor : Interceptor
{
    private readonly string _expectedApiKey;

    public ApiKeyAuthInterceptor()
    {
        _expectedApiKey = Environment.GetEnvironmentVariable("FCS_API_KEY") ??
                          throw new ArgumentException(
                              "Internal api key, needed to authenticate services requests missing.");
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        Authenticate(context);
        return await continuation(request, context);
    }

    private void Authenticate(ServerCallContext context)
    {
        if (!string.Equals(context.RequestHeaders.GetValue("X-API-KEY") ?? "", _expectedApiKey,
                StringComparison.Ordinal))
        {
            throw new ServiceUnauthenticatedException(
                "Invalid internal api-key metadata, needed to authenticate request to storage api.");
        }
    }
}