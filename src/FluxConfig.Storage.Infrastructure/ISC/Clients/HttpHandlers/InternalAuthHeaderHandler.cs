using FluxConfig.Storage.Infrastructure.ISC.Exceptions;

namespace FluxConfig.Storage.Infrastructure.ISC.Clients.HttpHandlers;

public class InternalAuthHeaderHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        string internalApiKey = Environment.GetEnvironmentVariable("FC_API_KEY")
                                ?? throw new InternalServiceUnauthenticatedException(
                                    "Invalid internal api-key metadata, needed to authenticate request to FC Management api.",
                                    apiKey: "MISSING",
                                    outgoing: true);


        request.Headers.Add("X-API-KEY", internalApiKey);

        return await base.SendAsync(request, cancellationToken);
    }
}