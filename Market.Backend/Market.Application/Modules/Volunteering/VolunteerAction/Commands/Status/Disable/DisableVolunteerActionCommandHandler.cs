using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Market.Application.Modules.Volunteering.Commands.Status.Disable;

public sealed class DisableVolunteerActionCommandHandler(IAppDbContext ctx)
    : IRequestHandler<DisableVolunteerActionCommand, Unit>
{
    public async Task<Unit> Handle(DisableVolunteerActionCommand request, CancellationToken ct)
    {
        var action = await ctx.VolunteerActions
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (action is null)
        {
            throw new MarketNotFoundException(
                $"Volunteer action (ID={request.Id}) nije pronađena.");
        }

        if (!action.IsEnabled)
            return Unit.Value; // idempotent

        action.IsEnabled = false;
        await ctx.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
