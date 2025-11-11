using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Identity;

namespace Market.Application.Modules.Identity.Roles.Queries.GetById;

public sealed class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, GetRoleByIdQueryDto>
{
    private readonly IAppDbContext _ctx;

    public GetRoleByIdQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<GetRoleByIdQueryDto> Handle(GetRoleByIdQuery request, CancellationToken ct)
    {
        var role = await _ctx.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == request.Id, ct);

        if (role == null)
            throw new MarketNotFoundException($"Role with Id {request.Id} not found.");

        return new GetRoleByIdQueryDto
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description
        };
    }
}
