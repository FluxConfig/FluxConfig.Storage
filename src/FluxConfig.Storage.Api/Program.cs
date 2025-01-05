using FluxConfig.Storage.Api.Extensions;

namespace FluxConfig.Storage.Api;

public sealed class Program
{
    public static async Task Main()
    {
        var builder = Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
            .ConfigureWebHost(webHost =>
            {
                webHost.ConfigureHttps();
            });

        await builder
            .Build()
            .RunAsync();
    }
}