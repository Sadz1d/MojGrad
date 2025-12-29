using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Volunteering;

namespace Market.Application.Modules.Volunteering.VolunteerActions.Queries.List;

public sealed class ListVolunteerActionsQueryHandler
    : IRequestHandler<ListVolunteerActionsQuery, PageResult<ListVolunteerActionsQueryDto>>
{
    private readonly IAppDbContext _ctx;
    public ListVolunteerActionsQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<PageResult<ListVolunteerActionsQueryDto>> Handle(
        ListVolunteerActionsQuery request, CancellationToken ct)
    {
        IQueryable<VolunteerActionEntity> q = _ctx.VolunteerActions.AsNoTracking();

        // 🔎 Search: name / description / location
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            q = q.Where(a =>
                a.Name.ToLower().Contains(term) ||
                (a.Description != null && a.Description.ToLower().Contains(term)) ||
                (a.Location != null && a.Location.ToLower().Contains(term)));
        }

        // 📅 Date filters
        if (request.DateFrom.HasValue)
            q = q.Where(a => a.EventDate >= request.DateFrom.Value);

        if (request.DateTo.HasValue)
            q = q.Where(a => a.EventDate <= request.DateTo.Value);

        // 🚀 Only upcoming
        if (request.OnlyUpcoming == true)
            q = q.Where(a => a.EventDate >= DateTime.UtcNow);

        // Projekcija (+ broj učesnika)
        var projected = q
            .OrderBy(a => a.EventDate)
            .Select(a => new ListVolunteerActionsQueryDto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                Location = a.Location,
                EventDate = a.EventDate,
                MaxParticipants = a.MaxParticipants,
                ParticipantsCount = _ctx.ActionParticipants.Count(p => p.ActionId == a.Id)
            });

        // 🎟️ Only with free slots (nakon projekcije)
        if (request.OnlyWithFreeSlots == true)
            projected = projected.Where(d => d.ParticipantsCount < d.MaxParticipants);

        return await PageResult<ListVolunteerActionsQueryDto>
            .FromQueryableAsync(projected, request.Paging, ct);
    }
}
