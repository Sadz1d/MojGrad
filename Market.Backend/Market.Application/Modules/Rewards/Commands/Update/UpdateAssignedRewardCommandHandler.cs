using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Rewards;

namespace Market.Application.Modules.Rewards.AssignedRewards.Commands.Update;

public sealed class UpdateAssignedRewardCommandHandler
    : IRequestHandler<UpdateAssignedRewardCommand, Unit>
{
    private readonly IAppDbContext _ctx;
    public UpdateAssignedRewardCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(UpdateAssignedRewardCommand request, CancellationToken ct)
    {
        var entity = await _ctx.AssignedRewards
            .FirstOrDefaultAsync(a => a.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"AssignedReward (Id={request.Id}) not found.");

        // ✅ ako želiš promijeniti reward
        if (request.RewardId.HasValue && request.RewardId.Value != entity.RewardId)
        {
            var rewardExists = await _ctx.Rewards.AnyAsync(r => r.Id == request.RewardId, ct);
            if (!rewardExists)
                throw new MarketNotFoundException($"Reward with ID {request.RewardId} not found.");

            entity.RewardId = request.RewardId.Value;
        }

        // ✅ ako želiš promijeniti datum dodjele
        if (request.AssignmentDate.HasValue)
            entity.AssignmentDate = request.AssignmentDate.Value;

        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
