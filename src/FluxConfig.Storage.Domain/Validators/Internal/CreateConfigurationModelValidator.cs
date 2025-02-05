using FluentValidation;
using FluxConfig.Storage.Domain.Models.Internal;

namespace FluxConfig.Storage.Domain.Validators.Internal;

public class CreateConfigurationModelValidator: AbstractValidator<CreateConfigurationModel>
{
    public CreateConfigurationModelValidator()
    {
        RuleFor(x => x.ConfigurationKey).NotEmpty().NotNull();
        RuleFor(x => x.ConfigurationTag).NotEmpty().NotNull();
    }
}