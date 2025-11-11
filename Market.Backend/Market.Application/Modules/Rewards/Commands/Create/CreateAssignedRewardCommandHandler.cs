using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Rewards;

namespace Market.Application.Modules.Rewards.AssignedRewards.Commands.Create;

public sealed class CreateAssignedRewardCommandHandler
    : IRequestHandler<CreateAssignedRewardCommand, int>
{
    private readonly IAppDbContext _ctx;
    public CreateAssignedRewardCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<int> Handle(CreateAssignedRewardCommand request, CancellationToken ct)
    {
        // 1) Postoji li user?
        var userExists = await _ctx.Users.AnyAsync(u => u.Id == request.UserId, ct);
        if (!userExists)
            throw new MarketNotFoundException($"User with ID {request.UserId} not found.");

        // 2) Postoji li reward?
        var rewardExists = await _ctx.Rewards.AnyAsync(r => r.Id == request.RewardId, ct);
        if (!rewardExists)
            throw new MarketNotFoundException($"Reward with ID {request.RewardId} not found.");

        // 3) Da li već postoji dodijeljena ista nagrada istom useru?
        var duplicate = await _ctx.AssignedRewards
            .AnyAsync(ar => ar.UserId == request.UserId && ar.RewardId == request.RewardId, ct);
        if (duplicate)
            throw new MarketConflictException("This reward is already assigned to the user.");

        var entity = new AssignedRewardEntity
        {
            UserId = request.UserId,
            RewardId = request.RewardId,
            AssignmentDate = request.AssignmentDate ?? DateTime.UtcNow
        };

        _ctx.AssignedRewards.Add(entity);
        await _ctx.SaveChangesAsync(ct);

        return entity.Id;
    }
}
