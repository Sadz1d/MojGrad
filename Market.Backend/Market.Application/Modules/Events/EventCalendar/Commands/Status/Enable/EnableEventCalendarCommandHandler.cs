using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Market.Application.Modules.Events.Commands.Status.Enable;

public sealed class EnableEventCalendarCommandHandler(IAppDbContext ctx)
    : IRequestHandler<EnableEventCalendarCommand, Unit>
{
    public async Task<Unit> Handle(EnableEventCalendarCommand request, CancellationToken ct)
    {
        var ev = await ctx.EventsCalendar
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (ev is null)
        {
            throw new MarketNotFoundException(
                $"Event (ID={request.Id}) nije pronađen.");
        }

        if (!ev.IsEnabled)
        {
            ev.IsEnabled = true;
            await ctx.SaveChangesAsync(ct);
        }

        return Unit.Value; // idempotent
    }
}
