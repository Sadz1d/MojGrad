using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Identity;

namespace Market.Application.Modules.Identity.Roles.Commands.Create;

public sealed class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, int>
{
    private readonly IAppDbContext _ctx;

    public CreateRoleCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<int> Handle(CreateRoleCommand request, CancellationToken ct)
    {
        // Opcionalno: provjera duplikata po imenu
        var exists = await _ctx.Roles
            .AnyAsync(r => r.Name.ToLower() == request.Name.Trim().ToLower(), ct);

        if (exists)
            throw new MarketConflictException($"Role with name '{request.Name}' already exists.");

        var entity = new RoleEntity
        {
            Name = request.Name.Trim(),
            Description = request.Description?.Trim()
        };

        _ctx.Roles.Add(entity);
        await _ctx.SaveChangesAsync(ct);

        return entity.Id;
    }
}
