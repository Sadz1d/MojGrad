using MediatR;

namespace Market.Application.Modules.Rewards.Commands.Status.Disable;

public sealed class DisableRewardCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}
