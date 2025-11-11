using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Events;

namespace Market.Application.Modules.Civic.Events.Commands.Create;

public sealed class CreateEventCalendarCommandHandler
    : IRequestHandler<CreateEventCalendarCommand, int>
{
    private readonly IAppDbContext _ctx;

    public CreateEventCalendarCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<int> Handle(CreateEventCalendarCommand request, CancellationToken ct)
    {
        var normalizedName = request.Name.Trim();

        // (opcionalno) zabrani duplikat događaja sa istim imenom i datumom
        var exists = await _ctx.EventsCalendar
            .AnyAsync(e => e.Name == normalizedName && e.EventDate == request.EventDate, ct);

        if (exists)
            throw new MarketConflictException("An event with the same name and date already exists.");

        var entity = new EventCalendarEntity
        {
            Name = normalizedName,
            Description = request.Description?.Trim(),
            EventDate = request.EventDate,
            EventType = request.EventType?.Trim()
        };

        _ctx.EventsCalendar.Add(entity);
        await _ctx.SaveChangesAsync(ct);

        return entity.Id;
    }
}
