// Market.Application/Modules/Rewards/AssignedRewards/Commands/Delete/DeleteAssignedRewardCommand.cs
using MediatR;

namespace Market.Application.Modules.Rewards.AssignedRewards.Commands.Delete;

public sealed class DeleteAssignedRewardCommand : IRequest<Unit>
{
    public required int Id { get; init; }
}
