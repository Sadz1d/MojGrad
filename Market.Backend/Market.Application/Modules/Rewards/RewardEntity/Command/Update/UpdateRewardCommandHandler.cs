using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Rewards;

namespace Market.Application.Modules.Rewards.Reward.Commands.Update;

public sealed class UpdateRewardCommandHandler
    : IRequestHandler<UpdateRewardCommand, Unit>
{
    private readonly IAppDbContext _ctx;
    public UpdateRewardCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(UpdateRewardCommand request, CancellationToken ct)
    {
        var entity = await _ctx.Rewards
            .FirstOrDefaultAsync(r => r.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"Reward (Id={request.Id}) not found.");

        // ✅ ažuriranje polja ako su poslata
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var name = request.Name.Trim();

            // provjera postoji li već reward s tim imenom
            var duplicate = await _ctx.Rewards
                .AnyAsync(r => r.Name == name && r.Id != entity.Id, ct);
            if (duplicate)
                throw new MarketConflictException($"Reward name '{name}' already exists.");

            entity.Name = name;
        }

        if (request.Description != null)
            entity.Description = request.Description.Trim();

        if (request.MinimumPoints.HasValue)
            entity.MinimumPoints = request.MinimumPoints.Value;

        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
