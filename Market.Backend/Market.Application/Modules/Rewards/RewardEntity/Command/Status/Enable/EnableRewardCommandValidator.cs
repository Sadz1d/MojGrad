using FluentValidation;

namespace Market.Application.Modules.Rewards.Commands.Status.Enable;

public sealed class EnableRewardCommandValidator
    : AbstractValidator<EnableRewardCommand>
{
    public EnableRewardCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
