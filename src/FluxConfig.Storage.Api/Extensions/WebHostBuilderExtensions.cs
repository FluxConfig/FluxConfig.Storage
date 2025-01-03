using System.Net;
using System.Security.Authentication;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace FluxConfig.Storage.Api.Extensions;

public static class WebHostBuilderExtensions
{
    public static void ConfigureHttps(this IWebHostBuilder webHostBuilder)
    {
        webHostBuilder.ConfigureKestrel((context, serverOptions) =>
        {
            serverOptions.ConfigureHttpsDefaults(listenOptions => { listenOptions.SslProtocols = SslProtocols.Tls13; });

            serverOptions.Listen(IPAddress.Any, 7077, options =>
            {
                options.Protocols = HttpProtocols.Http2;
                options.UseHttps(
                    fileName: Environment.GetEnvironmentVariable("INTERNAL_CERT_PATH") ??
                              throw new ArgumentException("Path to X.509 certificate file is missing."),
                    password: Environment.GetEnvironmentVariable("CERT_PSWD") ??
                              throw new ArgumentException("Password to access the X.509 certificate is missing.")
                );
            });
        });
    }
}