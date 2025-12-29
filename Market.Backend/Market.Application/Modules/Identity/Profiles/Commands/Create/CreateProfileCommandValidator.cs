using FluentValidation;
using Market.Application.Modules.Identity.Profiles.Commands.Create;

namespace Market.Application.Modules.Identity.Profile.Commands.Create;

public sealed class CreateProfileCommandValidator
    : AbstractValidator<CreateProfileCommand>
{
    public CreateProfileCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);

        RuleFor(x => x.Phone)
            .MaximumLength(30);

        RuleFor(x => x.Address)
            .MaximumLength(250);

        RuleFor(x => x.BiographyText)
            .MaximumLength(1000);
    }
}
