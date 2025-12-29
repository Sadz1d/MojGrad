using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Market.Application.Modules.Civic.CitizenProposals.Commands.Status.Disable;

public sealed class DisableCitizenProposalCommandHandler(IAppDbContext ctx)
    : IRequestHandler<DisableCitizenProposalCommand, Unit>
{
    public async Task<Unit> Handle(DisableCitizenProposalCommand request, CancellationToken ct)
    {
        var entity = await ctx.CitizenProposals
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"Citizen proposal (ID={request.Id}) not found.");

        if (!entity.IsEnabled)
            return Unit.Value; // idempotent

        entity.IsEnabled = false;
        await ctx.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
