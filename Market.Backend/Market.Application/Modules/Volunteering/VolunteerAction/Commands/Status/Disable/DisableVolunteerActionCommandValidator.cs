using FluentValidation;

namespace Market.Application.Modules.Volunteering.Commands.Status.Disable;

public sealed class DisableVolunteerActionCommandValidator
    : AbstractValidator<DisableVolunteerActionCommand>
{
    public DisableVolunteerActionCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
