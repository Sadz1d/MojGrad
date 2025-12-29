using FluentValidation;

namespace Market.Application.Modules.Rewards.Commands.Status.Disable;

public sealed class DisableRewardCommandValidator
    : AbstractValidator<DisableRewardCommand>
{
    public DisableRewardCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
