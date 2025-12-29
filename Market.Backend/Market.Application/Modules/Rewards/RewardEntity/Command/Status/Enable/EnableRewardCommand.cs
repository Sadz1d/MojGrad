using MediatR;

namespace Market.Application.Modules.Rewards.Commands.Status.Enable;

public sealed class EnableRewardCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}
