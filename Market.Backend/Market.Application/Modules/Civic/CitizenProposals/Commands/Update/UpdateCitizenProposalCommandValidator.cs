using FluentValidation;
using Market.Domain.Entities.Civic;

namespace Market.Application.Modules.Civic.CitizenProposals.Commands.Update;

public sealed class UpdateCitizenProposalCommandValidator
    : AbstractValidator<UpdateCitizenProposalCommand>
{
    public UpdateCitizenProposalCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(CitizenProposalEntity.Constraints.TitleMaxLength)
            .WithMessage($"Title can be at most {CitizenProposalEntity.Constraints.TitleMaxLength} characters long.");

        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Text is required.")
            .MaximumLength(CitizenProposalEntity.Constraints.TextMaxLength)
            .WithMessage($"Text can be at most {CitizenProposalEntity.Constraints.TextMaxLength} characters long.");
    }
}
