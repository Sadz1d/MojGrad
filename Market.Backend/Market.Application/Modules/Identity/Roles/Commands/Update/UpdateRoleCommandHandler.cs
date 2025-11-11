using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Identity;

namespace Market.Application.Modules.Identity.Roles.Commands.Update;

public sealed class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Unit>
{
    private readonly IAppDbContext _ctx;

    public UpdateRoleCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(UpdateRoleCommand request, CancellationToken ct)
    {
        var entity = await _ctx.Roles
            .FirstOrDefaultAsync(r => r.Id == request.Id, ct);

        if (entity == null)
            throw new MarketNotFoundException($"Role with Id {request.Id} not found.");

        // Opcionalno: provjera duplikata po imenu
        var exists = await _ctx.Roles
            .AnyAsync(r => r.Id != request.Id && r.Name.ToLower() == request.Name.Trim().ToLower(), ct);

        if (exists)
            throw new MarketConflictException($"Another role with name '{request.Name}' already exists.");

        entity.Name = request.Name.Trim();
        entity.Description = request.Description?.Trim();

        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
