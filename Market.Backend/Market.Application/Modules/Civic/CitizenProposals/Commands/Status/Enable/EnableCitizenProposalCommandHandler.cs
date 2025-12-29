using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Market.Application.Modules.Civic.CitizenProposals.Commands.Status.Enable;

public sealed class EnableCitizenProposalCommandHandler(IAppDbContext ctx)
    : IRequestHandler<EnableCitizenProposalCommand, Unit>
{
    public async Task<Unit> Handle(EnableCitizenProposalCommand request, CancellationToken ct)
    {
        var entity = await ctx.CitizenProposals
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"Citizen proposal (ID={request.Id}) not found.");

        if (!entity.IsEnabled)
        {
            entity.IsEnabled = true;
            await ctx.SaveChangesAsync(ct);
        }

        return Unit.Value;
    }
}
