using System.Data;
using FluentValidation;
using FluxConfig.Storage.Domain.Models.Internal;

namespace FluxConfig.Storage.Domain.Validators.Internal;

public class UpdateConfigurationModelValidator: AbstractValidator<UpdateConfigurationModel>
{
    public UpdateConfigurationModelValidator()
    {
        RuleFor(x => x.ConfigurationKey).NotEmpty().NotNull();
        RuleFor(x => x.ConfigurationTag).NotEmpty().NotNull();
        RuleFor(x => x.RawJsonConfigurationData).NotNull();
    }
}