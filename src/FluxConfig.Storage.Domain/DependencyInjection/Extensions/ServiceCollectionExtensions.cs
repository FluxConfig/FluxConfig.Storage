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
        services.AddScoped<IStorageInternalService, StorageInternalService>();
        return services;
    }

    private static void DisableValidationLocalization()
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;
    }
}