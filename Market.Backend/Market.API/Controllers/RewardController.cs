using Market.Application.Modules.Rewards.Reward.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers;

[ApiController]
[Route("api/rewards")]
public sealed class RewardController : ControllerBase
{
    private readonly ISender _sender;
    public RewardController(ISender sender) => _sender = sender;

    // GET /api/rewards?search=&minPoints=&maxPoints=&page=1&pageSize=20
    [HttpGet]
    public async Task<PageResult<ListRewardsQueryDto>> List(
        [FromQuery] ListRewardsQuery query,
        CancellationToken ct)
        => await _sender.Send(query, ct);
}
