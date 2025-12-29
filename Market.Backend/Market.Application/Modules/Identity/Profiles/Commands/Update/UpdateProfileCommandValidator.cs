using FluentValidation;
using Market.Application.Modules.Identity.Profiles.Commands.Update;

namespace Market.Application.Modules.Identity.Profile.Commands.Update;

public sealed class UpdateProfileCommandValidator
    : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Phone)
            .MaximumLength(30);

        RuleFor(x => x.Address)
            .MaximumLength(250);

        RuleFor(x => x.BiographyText)
            .MaximumLength(1000);
    }
}