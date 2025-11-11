using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Rewards;

namespace Market.Application.Modules.Rewards.Reward.Commands.Delete;

public sealed class DeleteRewardCommandHandler
    : IRequestHandler<DeleteRewardCommand, Unit>
{
    private readonly IAppDbContext _ctx;
    public DeleteRewardCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(DeleteRewardCommand request, CancellationToken ct)
    {
        var entity = await _ctx.Rewards
            .FirstOrDefaultAsync(r => r.Id == request.Id, ct);

        if (entity == null)
            throw new MarketNotFoundException($"Reward with Id {request.Id} not found.");

        // Provjeri ima li dodjela — ako ne želiš brisati nagradu koja je već dodijeljena
        var hasAssignments = await _ctx.AssignedRewards
            .AnyAsync(a => a.RewardId == entity.Id, ct);
        if (hasAssignments)
            throw new MarketConflictException("Cannot delete reward because it has assigned users.");

        _ctx.Rewards.Remove(entity);
        await _ctx.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
