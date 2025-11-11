// Market.Application/Modules/Rewards/AssignedRewards/Commands/Delete/DeleteAssignedRewardCommandHandler.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Rewards;

namespace Market.Application.Modules.Rewards.AssignedRewards.Commands.Delete;

public sealed class DeleteAssignedRewardCommandHandler
    : IRequestHandler<DeleteAssignedRewardCommand, Unit>
{
    private readonly IAppDbContext _ctx;

    public DeleteAssignedRewardCommandHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<Unit> Handle(DeleteAssignedRewardCommand request, CancellationToken ct)
    {
        var entity = await _ctx.AssignedRewards
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (entity == null)
            throw new MarketNotFoundException($"AssignedReward with Id {request.Id} not found.");

        _ctx.AssignedRewards.Remove(entity);
        await _ctx.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
