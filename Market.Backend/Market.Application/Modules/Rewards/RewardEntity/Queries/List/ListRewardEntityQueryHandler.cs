using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Rewards;

namespace Market.Application.Modules.Rewards.Reward.Queries.List;

public sealed class ListRewardsQueryHandler
    : IRequestHandler<ListRewardsQuery, PageResult<ListRewardsQueryDto>>
{
    private readonly IAppDbContext _ctx;
    public ListRewardsQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<PageResult<ListRewardsQueryDto>> Handle(
        ListRewardsQuery request, CancellationToken ct)
    {
        IQueryable<RewardEntity> q = _ctx.Rewards.AsNoTracking();

        // 🔎 Search po imenu/opisu
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            q = q.Where(r =>
                r.Name.ToLower().Contains(term) ||
                (r.Description != null && r.Description.ToLower().Contains(term)));
        }

        // 🎯 Filter MinimumPoints
        if (request.MinPoints.HasValue)
            q = q.Where(r => r.MinimumPoints >= request.MinPoints.Value);

        if (request.MaxPoints.HasValue)
            q = q.Where(r => r.MinimumPoints <= request.MaxPoints.Value);

        // Projekcija + broj dodjela (Assignments)
        var projected = q
            .OrderBy(r => r.MinimumPoints)
            .Select(r => new ListRewardsQueryDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                MinimumPoints = r.MinimumPoints,
                AssignmentsCount = _ctx.AssignedRewards.Count(a => a.RewardId == r.Id)
            });

        return await PageResult<ListRewardsQueryDto>
            .FromQueryableAsync(projected, request.Paging, ct);
    }
}
