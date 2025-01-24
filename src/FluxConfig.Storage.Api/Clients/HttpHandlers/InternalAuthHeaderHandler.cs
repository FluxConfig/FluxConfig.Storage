namespace FluxConfig.Storage.Api.Clients.HttpHandlers;

public class InternalAuthHeaderHandler : DelegatingHandler
{
    private readonly ILogger<InternalAuthHeaderHandler> _logger;

    public InternalAuthHeaderHandler(ILogger<InternalAuthHeaderHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        string? internalApiKey = Environment.GetEnvironmentVariable("FCM_API_KEY");
        
        if (internalApiKey == null)
        {
            ArgumentException exc = new ArgumentException(
                    "Internal api-key metadata, needed to authenticate request to authentication api is missing.");
            _logger.LogError(exc,
                "[{curDate}] Internal api-key metadata, needed to authenticate request to authentication api is missing.", DateTime.Now);
            throw exc;
        }

        request.Headers.Add("X-API-KEY", internalApiKey);

        return await base.SendAsync(request, cancellationToken);
    }
}