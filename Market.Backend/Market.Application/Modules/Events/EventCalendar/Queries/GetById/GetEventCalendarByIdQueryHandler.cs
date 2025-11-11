using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Events;

namespace Market.Application.Modules.Civic.Events.Queries.GetById;

public sealed class GetEventCalendarByIdQueryHandler
    : IRequestHandler<GetEventCalendarByIdQuery, GetEventCalendarByIdQueryDto>
{
    private readonly IAppDbContext _ctx;

    public GetEventCalendarByIdQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<GetEventCalendarByIdQueryDto> Handle(
        GetEventCalendarByIdQuery request, CancellationToken ct)
    {
        var entity = await _ctx.EventsCalendar
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == request.Id, ct);

        if (entity == null)
            throw new KeyNotFoundException($"EventCalendar with Id {request.Id} not found.");

        return new GetEventCalendarByIdQueryDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            EventDate = entity.EventDate,
            EventType = entity.EventType
        };
    }
}
