using FluentValidation;

namespace Market.Application.Modules.Events.Commands.Status.Enable;

public sealed class EnableEventCalendarCommandValidator
    : AbstractValidator<EnableEventCalendarCommand>
{
    public EnableEventCalendarCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
