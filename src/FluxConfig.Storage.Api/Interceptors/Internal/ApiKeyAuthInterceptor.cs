using FluxConfig.Storage.Infrastructure.ISC.Exceptions;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace FluxConfig.Storage.Api.Interceptors.Internal;


public class ApiKeyAuthInterceptor : Interceptor
{
    private readonly string _expectedApiKey = Environment.GetEnvironmentVariable("FC_API_KEY") ??
                                              throw new InternalServiceUnauthenticatedException(
                                                  "Internal api key needed to authenticate requests from FC Management service is missing.",
                                                  apiKey: "MISSING on FluxConfig.Storage service",
                                                  outgoing: true);

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        Authenticate(context);
        return await continuation(request, context);
    }

    private void Authenticate(ServerCallContext context)
    {
        string requestInternalApiKey = context.RequestHeaders.GetValue("X-API-KEY") ?? "";

        if (!string.Equals(requestInternalApiKey, _expectedApiKey, StringComparison.Ordinal))
        {
            throw new InternalServiceUnauthenticatedException(
                "Invalid internal api-key metadata, needed to authenticate request to FC Storage.",
                apiKey: requestInternalApiKey,
                outgoing: false);
        }
    }
}