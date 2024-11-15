using FluentValidation;
using FluxConfig.Storage.Domain.Models.Internal;

namespace FluxConfig.Storage.Domain.Validators.Internal;

public class ChangeConfigTagModelValidator: AbstractValidator<ChangeConfigTagModel>
{
    public ChangeConfigTagModelValidator()
    {
        RuleFor(x => x.ConfigurationKey).NotEmpty().NotNull();
        RuleFor(x => x.OldConfigurationTag).NotEmpty().NotNull();
        RuleFor(x => x.NewConfigurationTag).NotEmpty().NotNull().NotEqual(x => x.NewConfigurationTag);
    }
    
}