using FluentValidation;
using FluxConfig.Storage.Domain.Models.Public;

namespace FluxConfig.Storage.Domain.Validators.Public;

public class LoadConfigurationModelValidator: AbstractValidator<LoadConfigurationModel>
{
    public LoadConfigurationModelValidator()
    {
        RuleFor(x => x.ConfigurationKey).NotEmpty().NotNull();
        RuleFor(x => x.ConfigurationTag).NotEmpty().NotNull();
    }
}