using FluentValidation;
using Market.Domain.Entities.Civic;

namespace Market.Application.Modules.Civic.CitizenProposals.Commands.Create;

public sealed class CreateCitizenProposalCommandValidator
    : AbstractValidator<CreateCitizenProposalCommand>
{
    public CreateCitizenProposalCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(CitizenProposalEntity.Constraints.TitleMaxLength)
            .WithMessage($"Title can be at most {CitizenProposalEntity.Constraints.TitleMaxLength} characters long.");

        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Text is required.")
            .MaximumLength(CitizenProposalEntity.Constraints.TextMaxLength)
            .WithMessage($"Text can be at most {CitizenProposalEntity.Constraints.TextMaxLength} characters long.");

        RuleFor(x => x.UserId)
            .GreaterThan(0);
    }
}
