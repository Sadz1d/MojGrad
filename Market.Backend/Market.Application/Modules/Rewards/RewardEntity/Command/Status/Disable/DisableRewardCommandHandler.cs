using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Market.Application.Modules.Rewards.Commands.Status.Disable;

public sealed class DisableRewardCommandHandler(IAppDbContext ctx)
    : IRequestHandler<DisableRewardCommand, Unit>
{
    public async Task<Unit> Handle(DisableRewardCommand request, CancellationToken ct)
    {
        var reward = await ctx.Rewards
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (reward is null)
        {
            throw new MarketNotFoundException(
                $"Reward (ID={request.Id}) nije pronađen.");
        }

        if (!reward.IsEnabled)
            return Unit.Value; // idempotent

        reward.IsEnabled = false;

        await ctx.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
