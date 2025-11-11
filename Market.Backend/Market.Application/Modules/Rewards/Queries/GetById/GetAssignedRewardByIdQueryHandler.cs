// Market.Application/Modules/Rewards/AssignedRewards/Queries/GetById/GetAssignedRewardByIdQueryHandler.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Rewards;

namespace Market.Application.Modules.Rewards.AssignedRewards.Queries.GetById;

public sealed class GetAssignedRewardByIdQueryHandler
    : IRequestHandler<GetAssignedRewardByIdQuery, GetAssignedRewardByIdQueryDto>
{
    private readonly IAppDbContext _ctx;
    public GetAssignedRewardByIdQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<GetAssignedRewardByIdQueryDto> Handle(
        GetAssignedRewardByIdQuery request, CancellationToken ct)
    {
        var assignedReward = await _ctx.AssignedRewards
            .AsNoTracking()
            .Include(ar => ar.User)
            .Include(ar => ar.Reward)
            .Where(ar => ar.Id == request.Id)
            .Select(ar => new GetAssignedRewardByIdQueryDto
            {
                Id = ar.Id,
                UserName = (ar.User.FirstName + " " + ar.User.LastName).Trim(),
                RewardName = ar.Reward.Name,  // ili Title, ako je tako u tvojoj RewardEntity
                AssignmentDate = ar.AssignmentDate
            })
            .FirstOrDefaultAsync(ct);

        if (assignedReward is null)
            throw new MarketNotFoundException($"AssignedReward with Id {request.Id} not found.");

        return assignedReward;
    }
}
