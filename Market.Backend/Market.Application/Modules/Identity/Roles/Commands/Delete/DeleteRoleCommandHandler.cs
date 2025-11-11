using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Identity;

namespace Market.Application.Modules.Identity.Roles.Commands.Delete;

public sealed class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Unit>
{
    private readonly IAppDbContext _ctx;

    public DeleteRoleCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(DeleteRoleCommand request, CancellationToken ct)
    {
        var role = await _ctx.Roles
            .Include(r => r.UserRoles)
            .FirstOrDefaultAsync(r => r.Id == request.Id, ct);

        if (role == null)
            throw new MarketNotFoundException($"Role with Id {request.Id} not found.");

        if (role.UserRoles.Any())
            throw new MarketConflictException("Cannot delete role because it is assigned to one or more users.");

        _ctx.Roles.Remove(role);
        await _ctx.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
