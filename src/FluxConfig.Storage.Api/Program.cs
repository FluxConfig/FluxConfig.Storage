namespace FluxConfig.Storage.Api;

public sealed class Program
{
    public static async Task Main()
    {
        var builder = Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());

        await builder
            .Build()
            .RunAsync();
    }
}