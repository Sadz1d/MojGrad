// Market.Application/Modules/Rewards/AssignedRewards/Queries/GetById/GetAssignedRewardByIdQuery.cs
using MediatR;

namespace Market.Application.Modules.Rewards.AssignedRewards.Queries.GetById;

public sealed class GetAssignedRewardByIdQuery : IRequest<GetAssignedRewardByIdQueryDto>
{
    public int Id { get; init; }
}
