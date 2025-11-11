using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Identity;

namespace Market.Application.Modules.Identity.UserRoles.Commands.Delete;

public sealed class DeleteUserRoleCommandHandler
    : IRequestHandler<DeleteUserRoleCommand, Unit>
{
    private readonly IAppDbContext _ctx;

    public DeleteUserRoleCommandHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<Unit> Handle(DeleteUserRoleCommand request, CancellationToken ct)
    {
        var entity = await _ctx.UserRoles
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (entity == null)
            throw new MarketNotFoundException($"UserRole with Id {request.Id} not found.");

        _ctx.UserRoles.Remove(entity);
        await _ctx.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
