using FluentValidation;
using Market.Application.Modules.Civic.Events.Commands.Create;
using Market.Domain.Entities.Events;

namespace Market.Application.Modules.Events.Commands.Create;

public sealed class CreateEventCalendarCommandValidator
    : AbstractValidator<CreateEventCalendarCommand>
{
    public CreateEventCalendarCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(EventCalendarEntity.Constraints.NameMaxLength)
            .WithMessage($"Name can be at most {EventCalendarEntity.Constraints.NameMaxLength} characters long.");

        RuleFor(x => x.Description)
            .MaximumLength(EventCalendarEntity.Constraints.DescriptionMaxLength)
            .WithMessage($"Description can be at most {EventCalendarEntity.Constraints.DescriptionMaxLength} characters long.");

        RuleFor(x => x.EventType)
            .MaximumLength(EventCalendarEntity.Constraints.TypeMaxLength)
            .WithMessage($"EventType can be at most {EventCalendarEntity.Constraints.TypeMaxLength} characters long.");

        RuleFor(x => x.EventDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Event date must be in the future.");
    }
}
