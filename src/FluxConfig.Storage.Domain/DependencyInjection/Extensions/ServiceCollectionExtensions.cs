using FluentValidation;
using FluxConfig.Storage.Domain.Services;
using FluxConfig.Storage.Domain.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FluxConfig.Storage.Domain.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {   
        DisableValidationLocalization();
        
        services.AddScoped<IStoragePublicService, StoragePublicService>();
        return services;
    }

    private static void DisableValidationLocalization()
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;
    }
}