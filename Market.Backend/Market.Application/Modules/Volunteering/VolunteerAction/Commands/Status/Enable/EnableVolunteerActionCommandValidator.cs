using FluentValidation;

namespace Market.Application.Modules.Volunteering.Commands.Status.Enable;

public sealed class EnableVolunteerActionCommandValidator
    : AbstractValidator<EnableVolunteerActionCommand>
{
    public EnableVolunteerActionCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
