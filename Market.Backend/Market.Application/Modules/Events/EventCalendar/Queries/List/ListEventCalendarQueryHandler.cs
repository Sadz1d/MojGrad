using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Events;

namespace Market.Application.Modules.Events.EventCalendar.Queries.List;

public sealed class ListEventCalendarQueryHandler
    : IRequestHandler<ListEventCalendarQuery, PageResult<ListEventCalendarQueryDto>>
{
    private readonly IAppDbContext _ctx;

    public ListEventCalendarQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<PageResult<ListEventCalendarQueryDto>> Handle(
        ListEventCalendarQuery request, CancellationToken ct)
    {
        IQueryable<EventCalendarEntity> q = _ctx.EventsCalendar.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            q = q.Where(e =>
                e.Name.ToLower().Contains(term) ||
                (e.Description != null && e.Description.ToLower().Contains(term)) ||
                (e.EventType != null && e.EventType.ToLower().Contains(term)));
        }

        if (request.OnlyUpcoming.HasValue && request.OnlyUpcoming.Value)
        {
            var now = DateTime.UtcNow;
            q = q.Where(e => e.EventDate >= now);
        }

        var projected = q
            .OrderBy(e => e.EventDate)
            .Select(e => new ListEventCalendarQueryDto
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                EventDate = e.EventDate,
                EventType = e.EventType
            });

        return await PageResult<ListEventCalendarQueryDto>
            .FromQueryableAsync(projected, request.Paging, ct);
    }
}
