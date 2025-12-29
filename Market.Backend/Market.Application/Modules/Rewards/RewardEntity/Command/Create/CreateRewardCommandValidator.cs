using FluentValidation;
using Market.Application.Modules.Rewards.Reward.Commands.Create;
using Market.Domain.Entities.Rewards;

namespace Market.Application.Modules.Rewards.Commands.Create;

public sealed class CreateRewardCommandValidator
    : AbstractValidator<CreateRewardCommand>
{
    public CreateRewardCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(RewardEntity.Constraints.NameMaxLength)
            .WithMessage($"Name can be at most {RewardEntity.Constraints.NameMaxLength} characters long.");

        RuleFor(x => x.Description)
            .MaximumLength(RewardEntity.Constraints.DescriptionMaxLength)
            .WithMessage($"Description can be at most {RewardEntity.Constraints.DescriptionMaxLength} characters long.");

        RuleFor(x => x.MinimumPoints)
            .GreaterThanOrEqualTo(0)
            .WithMessage("MinimumPoints must be zero or greater.");
    }
}
