using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Rewards;

namespace Market.Application.Modules.Rewards.Reward.Queries.GetById;

public sealed class GetRewardByIdQueryHandler
    : IRequestHandler<GetRewardByIdQuery, GetRewardByIdQueryDto>
{
    private readonly IAppDbContext _ctx;
    public GetRewardByIdQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<GetRewardByIdQueryDto> Handle(
        GetRewardByIdQuery request, CancellationToken ct)
    {
        var dto = await _ctx.Rewards
            .AsNoTracking()
            .Where(r => r.Id == request.Id)
            .Select(r => new GetRewardByIdQueryDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                MinimumPoints = r.MinimumPoints,
                AssignmentsCount = _ctx.AssignedRewards.Count(a => a.RewardId == r.Id)
            })
            .FirstOrDefaultAsync(ct);

        if (dto is null)
            throw new MarketNotFoundException($"Reward with Id {request.Id} not found.");

        return dto;
    }
}
