using MediatR;

namespace Market.Application.Modules.Rewards.Reward.Queries.GetById;

public sealed class GetRewardByIdQuery : IRequest<GetRewardByIdQueryDto>
{
    public int Id { get; init; }
}
