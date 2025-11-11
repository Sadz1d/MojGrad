using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Identity;

namespace Market.Application.Modules.Identity.UserRoles.Commands.Create;

public sealed class CreateUserRoleHandler : IRequestHandler<CreateUserRoleCommand, int>
{
    private readonly IAppDbContext _ctx;

    public CreateUserRoleHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<int> Handle(CreateUserRoleCommand request, CancellationToken ct)
    {
        // Provjera da li user i role postoje
        var userExists = await _ctx.Users.AnyAsync(u => u.Id == request.UserId, ct);
        var roleExists = await _ctx.Roles.AnyAsync(r => r.Id == request.RoleId, ct);

        if (!userExists)
            throw new MarketNotFoundException($"User with Id {request.UserId} not found.");
        if (!roleExists)
            throw new MarketNotFoundException($"Role with Id {request.RoleId} not found.");

        // Provjera da li već postoji ista kombinacija
        var exists = await _ctx.UserRoles
            .AnyAsync(ur => ur.UserId == request.UserId && ur.RoleId == request.RoleId, ct);

        if (exists)
            throw new MarketConflictException("This user already has the specified role.");

        var entity = new UserRoleEntity
        {
            UserId = request.UserId,
            RoleId = request.RoleId
        };

        _ctx.UserRoles.Add(entity);
        await _ctx.SaveChangesAsync(ct);

        return entity.Id;
    }
}
