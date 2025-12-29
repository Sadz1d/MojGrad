using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Market.Application.Modules.Events.Commands.Status.Disable;

public sealed class DisableEventCalendarCommandHandler(IAppDbContext ctx)
    : IRequestHandler<DisableEventCalendarCommand, Unit>
{
    public async Task<Unit> Handle(DisableEventCalendarCommand request, CancellationToken ct)
    {
        var ev = await ctx.EventsCalendar
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (ev is null)
        {
            throw new MarketNotFoundException(
                $"Event (ID={request.Id}) nije pronađen.");
        }

        if (!ev.IsEnabled)
            return Unit.Value; // idempotent

        ev.IsEnabled = false;
        await ctx.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
