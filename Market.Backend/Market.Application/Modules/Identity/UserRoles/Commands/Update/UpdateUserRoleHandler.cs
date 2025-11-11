using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Identity;

namespace Market.Application.Modules.Identity.UserRoles.Commands.Update;

public sealed class UpdateUserRoleHandler : IRequestHandler<UpdateUserRoleCommand, Unit>
{
    private readonly IAppDbContext _ctx;

    public UpdateUserRoleHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(UpdateUserRoleCommand request, CancellationToken ct)
    {
        var entity = await _ctx.UserRoles.FirstOrDefaultAsync(x => x.Id == request.Id, ct)
            ?? throw new MarketNotFoundException($"UserRole with Id {request.Id} not found.");

        var userExists = await _ctx.Users.AnyAsync(u => u.Id == request.UserId, ct);
        var roleExists = await _ctx.Roles.AnyAsync(r => r.Id == request.RoleId, ct);

        if (!userExists)
            throw new MarketNotFoundException($"User with Id {request.UserId} not found.");
        if (!roleExists)
            throw new MarketNotFoundException($"Role with Id {request.RoleId} not found.");

        var duplicate = await _ctx.UserRoles
            .AnyAsync(x => x.UserId == request.UserId && x.RoleId == request.RoleId && x.Id != request.Id, ct);

        if (duplicate)
            throw new MarketConflictException("This user already has that role.");

        entity.UserId = request.UserId;
        entity.RoleId = request.RoleId;

        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
