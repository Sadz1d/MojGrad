using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Identity;

namespace Market.Application.Modules.Identity.UserRoles.Queries.GetById;

public sealed class GetUserRoleByIdHandler
    : IRequestHandler<GetUserRoleByIdQuery, GetUserRoleByIdDto>
{
    private readonly IAppDbContext _ctx;

    public GetUserRoleByIdHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<GetUserRoleByIdDto> Handle(GetUserRoleByIdQuery request, CancellationToken ct)
    {
        var entity = await _ctx.UserRoles
            .AsNoTracking()
            .Include(ur => ur.User)
            .Include(ur => ur.Role)
            .FirstOrDefaultAsync(ur => ur.Id == request.Id, ct);

        if (entity == null)
            throw new MarketNotFoundException($"UserRole with Id {request.Id} not found.");

        return new GetUserRoleByIdDto
        {
            Id = entity.Id,
            UserId = entity.UserId,
            RoleId = entity.RoleId,
            UserName = entity.User != null
                ? (entity.User.FirstName + " " + entity.User.LastName).Trim()
                : "N/A",
            RoleName = entity.Role != null ? entity.Role.Name : "N/A"
        };
    }
}
