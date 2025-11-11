using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Identity;

namespace Market.Application.Modules.Identity.Profiles.Queries.List;

public sealed class ListProfilesQueryHandler
    : IRequestHandler<ListProfilesQuery, PageResult<ListProfilesQueryDto>>
{
    private readonly IAppDbContext _ctx;

    public ListProfilesQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<PageResult<ListProfilesQueryDto>> Handle(
        ListProfilesQuery request, CancellationToken ct)
    {
        IQueryable<ProfileEntity> q = _ctx.Profiles
            .AsNoTracking()
            .Include(p => p.User); // Potrebno za UserFullName

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            q = q.Where(p =>
                p.Address != null && p.Address.ToLower().Contains(term) ||
                p.Phone != null && p.Phone.ToLower().Contains(term) ||
                p.BiographyText != null && p.BiographyText.ToLower().Contains(term) ||
                p.User.FirstName.ToLower().Contains(term) ||
                p.User.LastName.ToLower().Contains(term));
        }

        var projected = q
            .OrderBy(p => p.UserId)
            .Select(p => new ListProfilesQueryDto
            {
                Id = p.Id,
                UserId = p.UserId,
                Address = p.Address,
                Phone = p.Phone,
                ProfilePicture = p.ProfilePicture,
                BiographyText = p.BiographyText,
                UserFullName = (p.User.FirstName + " " + p.User.LastName).Trim()
            });

        return await PageResult<ListProfilesQueryDto>
            .FromQueryableAsync(projected, request.Paging, ct);
    }
}
