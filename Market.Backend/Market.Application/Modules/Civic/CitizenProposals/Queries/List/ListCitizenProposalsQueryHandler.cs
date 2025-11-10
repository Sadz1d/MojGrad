using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Civic;

namespace Market.Application.Modules.Civic.CitizenProposals.Queries.List;

public sealed class ListCitizenProposalsQueryHandler
    : IRequestHandler<ListCitizenProposalsQuery, PageResult<ListCitizenProposalsQueryDto>>
{
    private readonly IAppDbContext _ctx; // isto kao u Products handleru

    public ListCitizenProposalsQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<PageResult<ListCitizenProposalsQueryDto>> Handle(
        ListCitizenProposalsQuery request, CancellationToken ct)
    {
        IQueryable<CitizenProposalEntity> q = _ctx.CitizenProposals
            .AsNoTracking()
            .Include(p => p.User); // treba ti za AuthorName

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            q = q.Where(p =>
                p.Title.ToLower().Contains(term) ||
                p.Text.ToLower().Contains(term));
        }

        if (request.OnlyEnabled.HasValue)
            q = q.Where(p => p.IsEnabled == request.OnlyEnabled.Value);

        var projected = q
            .OrderByDescending(p => p.PublicationDate)
            .Select(p => new ListCitizenProposalsQueryDto
            {
                Id = p.Id,
                Title = p.Title,
                AuthorName = p.User != null
                    ? (p.User.FirstName + " " + p.User.LastName).Trim()
                    : "Anonimno",
                PublicationDate = p.PublicationDate,
                ShortText = p.Text.Length > 120 ? p.Text.Substring(0, 120) + "..." : p.Text,
                IsEnabled = p.IsEnabled
            });

        return await PageResult<ListCitizenProposalsQueryDto>
            .FromQueryableAsync(projected, request.Paging, ct);
    }
}
