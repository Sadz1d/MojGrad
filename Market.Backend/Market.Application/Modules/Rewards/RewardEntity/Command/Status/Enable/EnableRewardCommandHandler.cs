using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Market.Application.Modules.Rewards.Commands.Status.Enable;

public sealed class EnableRewardCommandHandler(IAppDbContext ctx)
    : IRequestHandler<EnableRewardCommand, Unit>
{
    public async Task<Unit> Handle(EnableRewardCommand request, CancellationToken ct)
    {
        var reward = await ctx.Rewards
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (reward is null)
        {
            throw new MarketNotFoundException(
                $"Reward (ID={request.Id}) nije pronađen.");
        }

        if (!reward.IsEnabled)
        {
            reward.IsEnabled = true;
            await ctx.SaveChangesAsync(ct);
        }

        return Unit.Value; // idempotent
    }
}
