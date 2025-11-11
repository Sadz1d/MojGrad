using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Volunteering;

namespace Market.Application.Modules.Volunteering.ActionParticipants.Queries.List;

public sealed class ListActionParticipantsQueryHandler
    : IRequestHandler<ListActionParticipantsQuery, PageResult<ListActionParticipantsQueryDto>>
{
    private readonly IAppDbContext _ctx;
    public ListActionParticipantsQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<PageResult<ListActionParticipantsQueryDto>> Handle(
        ListActionParticipantsQuery request, CancellationToken ct)
    {
        IQueryable<ActionParticipantEntity> q = _ctx.ActionParticipants
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Action);

        // 🔎 Search po korisniku ili nazivu akcije
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            q = q.Where(x =>
                (x.User != null && (x.User.FirstName + " " + x.User.LastName).ToLower().Contains(term)) ||
                (x.Action != null && x.Action.Name.ToLower().Contains(term)));
        }

        // 🎯 Filter po korisniku i akciji
        if (request.UserId.HasValue)
            q = q.Where(x => x.UserId == request.UserId);

        if (request.ActionId.HasValue)
            q = q.Where(x => x.ActionId == request.ActionId);

        // 📋 Projekcija
        var projected = q
            .OrderByDescending(x => x.RegistrationDate)
            .Select(x => new ListActionParticipantsQueryDto
            {
                Id = x.Id,
                UserName = x.User != null
                    ? (x.User.FirstName + " " + x.User.LastName).Trim()
                    : "(Unknown user)",
                ActionTitle = x.Action != null ? x.Action.Name : "(Unknown action)",
                RegistrationDate = x.RegistrationDate
            });

        return await PageResult<ListActionParticipantsQueryDto>
            .FromQueryableAsync(projected, request.Paging, ct);
    }
}
