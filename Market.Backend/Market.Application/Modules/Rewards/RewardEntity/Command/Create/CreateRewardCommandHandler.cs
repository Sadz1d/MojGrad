using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Rewards;

namespace Market.Application.Modules.Rewards.Reward.Commands.Create;

public sealed class CreateRewardCommandHandler
    : IRequestHandler<CreateRewardCommand, int>
{
    private readonly IAppDbContext _ctx;
    public CreateRewardCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<int> Handle(CreateRewardCommand request, CancellationToken ct)
    {
        var normalizedName = request.Name.Trim();

        // ✅ Provjera da li već postoji reward s istim imenom
        var exists = await _ctx.Rewards.AnyAsync(r => r.Name == normalizedName, ct);
        if (exists)
            throw new MarketConflictException($"Reward '{request.Name}' already exists.");

        // ✅ Kreiraj novu nagradu
        var entity = new RewardEntity
        {
            Name = normalizedName,
            Description = request.Description?.Trim(),
            MinimumPoints = request.MinimumPoints
        };

        _ctx.Rewards.Add(entity);
        await _ctx.SaveChangesAsync(ct);

        return entity.Id;
    }
}
