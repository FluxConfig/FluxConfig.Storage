using FluentValidation;
using FluxConfig.Storage.Domain.Models.Internal;

namespace FluxConfig.Storage.Domain.Validators.Internal;

public class CopyConfigurationModelValidator: AbstractValidator<CopyConfigurationModel>
{
    public CopyConfigurationModelValidator()
    {
        RuleFor(x => x.ConfigurationKey).NotNull().NotEmpty();
        RuleFor(x => x.SourceConfigurationTag).NotNull().NotEmpty();
        RuleFor(x => x.DestConfigurationTag).NotNull().NotEmpty().NotEqual(x => x.SourceConfigurationTag);
    }
}