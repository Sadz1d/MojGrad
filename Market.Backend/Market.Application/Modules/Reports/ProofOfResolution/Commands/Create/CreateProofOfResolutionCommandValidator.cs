using FluentValidation;

namespace Market.Application.Modules.Reports.ProofOfResolution.Commands.Create;

public sealed class CreateProofOfResolutionCommandValidator
    : AbstractValidator<CreateProofOfResolutionCommand>
{
    public CreateProofOfResolutionCommandValidator()
    {
        RuleFor(x => x.TaskId)
            .GreaterThan(0);
    }
}
