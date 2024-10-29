using FluxConfig.Storage.Api.Extensions;
using FluxConfig.Storage.Domain.DependencyInjection.Extensions;

namespace FluxConfig.Storage.Api;

public sealed class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddGrpcWithInterceptors(_environment)
            .AddDomainServices();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseEndpoints(builder =>
        {
            builder.MapGrpcServices();
        });
    }
}