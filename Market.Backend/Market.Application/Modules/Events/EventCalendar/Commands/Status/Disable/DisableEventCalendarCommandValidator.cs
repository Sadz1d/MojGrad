using FluentValidation;

namespace Market.Application.Modules.Events.Commands.Status.Disable;

public sealed class DisableEventCalendarCommandValidator
    : AbstractValidator<DisableEventCalendarCommand>
{
    public DisableEventCalendarCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
