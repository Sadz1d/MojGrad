using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Events;

namespace Market.Application.Modules.Events.EventCalendar.Commands.Delete;

public sealed class DeleteEventCalendarCommandHandler
    : IRequestHandler<DeleteEventCalendarCommand, Unit>
{
    private readonly IAppDbContext _ctx;

    public DeleteEventCalendarCommandHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<Unit> Handle(DeleteEventCalendarCommand request, CancellationToken ct)
    {
        var ev = await _ctx.EventsCalendar
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (ev == null)
            throw new MarketNotFoundException($"Event with Id {request.Id} not found.");

        _ctx.EventsCalendar.Remove(ev);
        await _ctx.SaveChangesAsync(ct);

        return Unit.Value;
    }
}