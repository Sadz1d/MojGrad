using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Identity;

namespace Market.Application.Modules.Identity.UserRoles.Queries.List;

public sealed class ListUserRolesHandler
    : IRequestHandler<ListUserRolesQuery, PageResult<ListUserRolesDto>>
{
    private readonly IAppDbContext _ctx;

    public ListUserRolesHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<PageResult<ListUserRolesDto>> Handle(ListUserRolesQuery request, CancellationToken ct)
    {
        IQueryable<UserRoleEntity> q = _ctx.UserRoles
            .AsNoTracking()
            .Include(ur => ur.User)
            .Include(ur => ur.Role);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            q = q.Where(ur =>
                (ur.User.FirstName + " " + ur.User.LastName).ToLower().Contains(term) ||
                ur.Role.Name.ToLower().Contains(term));
        }

        var projected = q
            .OrderBy(ur => ur.Id)
            .Select(ur => new ListUserRolesDto
            {
                Id = ur.Id,
                UserId = ur.UserId,
                RoleId = ur.RoleId,
                UserName = ur.User != null
                    ? (ur.User.FirstName + " " + ur.User.LastName).Trim()
                    : "N/A",
                RoleName = ur.Role != null ? ur.Role.Name : "N/A"
            });

        return await PageResult<ListUserRolesDto>.FromQueryableAsync(projected, request.Paging, ct);
    }
}
