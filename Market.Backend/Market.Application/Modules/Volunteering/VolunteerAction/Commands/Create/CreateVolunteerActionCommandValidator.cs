using FluentValidation;
using Market.Application.Modules.Volunteering.VolunteerActions.Commands.Create;
using Market.Domain.Entities.Volunteering;

namespace Market.Application.Modules.Volunteering.Commands.Create;

public sealed class CreateVolunteerActionCommandValidator
    : AbstractValidator<CreateVolunteerActionCommand>
{
    public CreateVolunteerActionCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(VolunteerActionEntity.Constraints.NameMaxLength)
            .WithMessage($"Name can be at most {VolunteerActionEntity.Constraints.NameMaxLength} characters long.");

        RuleFor(x => x.Description)
            .MaximumLength(VolunteerActionEntity.Constraints.DescriptionMaxLength)
            .WithMessage($"Description can be at most {VolunteerActionEntity.Constraints.DescriptionMaxLength} characters long.");

        RuleFor(x => x.Location)
            .MaximumLength(VolunteerActionEntity.Constraints.LocationMaxLength)
            .WithMessage($"Location can be at most {VolunteerActionEntity.Constraints.LocationMaxLength} characters long.");

        RuleFor(x => x.EventDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Event date must be in the future.");

        RuleFor(x => x.MaxParticipants)
            .GreaterThan(0)
            .WithMessage("MaxParticipants must be greater than zero.");
    }
}
