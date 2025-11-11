// Market.Application/Modules/Rewards/AssignedRewards/Queries/List/ListAssignedRewardsQueryHandler.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Rewards;

namespace Market.Application.Modules.Rewards.AssignedRewards.Queries.List;

public sealed class ListAssignedRewardsQueryHandler
    : IRequestHandler<ListAssignedRewardsQuery, PageResult<ListAssignedRewardsQueryDto>>
{
    private readonly IAppDbContext _ctx;
    public ListAssignedRewardsQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<PageResult<ListAssignedRewardsQueryDto>> Handle(
        ListAssignedRewardsQuery request, CancellationToken ct)
    {
        IQueryable<AssignedRewardEntity> q = _ctx.AssignedRewards   // DbSet<AssignedRewardEntity>
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Reward);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            q = q.Where(x =>
                (x.User.FirstName + " " + x.User.LastName).ToLower().Contains(term) ||
                x.Reward.Name.ToLower().Contains(term)); // ili Title, zavisi od tvoje RewardEntity
        }

        if (request.UserId.HasValue)
            q = q.Where(x => x.UserId == request.UserId.Value);

        if (request.RewardId.HasValue)
            q = q.Where(x => x.RewardId == request.RewardId.Value);

        var projected = q
            .OrderByDescending(x => x.AssignmentDate)
            .Select(x => new ListAssignedRewardsQueryDto
            {
                Id = x.Id,
                UserName = (x.User.FirstName + " " + x.User.LastName).Trim(),
                RewardName = x.Reward.Name,        // prilagodi polju u RewardEntity (Name/Title)
                AssignmentDate = x.AssignmentDate
            });

        return await PageResult<ListAssignedRewardsQueryDto>
            .FromQueryableAsync(projected, request.Paging, ct);
    }
}
