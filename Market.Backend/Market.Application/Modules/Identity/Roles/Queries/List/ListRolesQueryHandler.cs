using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Identity;

namespace Market.Application.Modules.Identity.Roles.Queries.List;

public sealed class ListRolesQueryHandler : IRequestHandler<ListRolesQuery, IEnumerable<ListRolesQueryDto>>
{
    private readonly IAppDbContext _ctx;

    public ListRolesQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<ListRolesQueryDto>> Handle(ListRolesQuery request, CancellationToken ct)
    {
        return await _ctx.Roles
            .AsNoTracking()
            .Select(r => new ListRolesQueryDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description
            })
            .ToListAsync(ct);
    }
}
