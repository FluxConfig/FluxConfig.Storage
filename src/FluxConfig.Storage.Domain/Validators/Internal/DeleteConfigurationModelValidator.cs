using FluentValidation;
using FluxConfig.Storage.Domain.Models.Internal;

namespace FluxConfig.Storage.Domain.Validators.Internal;

public class DeleteConfigurationModelValidator: AbstractValidator<DeleteConfigurationModel>
{
    public DeleteConfigurationModelValidator()
    {
        RuleFor(x => x.ConfigurationKey).NotEmpty().NotNull();
        RuleFor(x => x.ConfigurationTags).NotEmpty().NotNull();
        RuleForEach(x => x.ConfigurationTags).ChildRules(tag =>
        {
            tag.RuleFor(x => x).NotEmpty().NotNull();
        });
    }
}