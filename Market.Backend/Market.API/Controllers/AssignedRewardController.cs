// Market.API/Controllers/AssignedRewardsController.cs
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Market.Application.Modules.Rewards.AssignedRewards.Queries.List;

namespace Market.API.Controllers;

[ApiController]
[Route("api/rewards/assigned")]
public sealed class AssignedRewardsController : ControllerBase
{
    private readonly ISender _sender;
    public AssignedRewardsController(ISender sender) => _sender = sender;

    // GET /api/rewards/assigned?search=...&userId=...&rewardId=...&page=1&pageSize=20
    [HttpGet]
    public async Task<PageResult<ListAssignedRewardsQueryDto>> List(
        [FromQuery] ListAssignedRewardsQuery query, CancellationToken ct)
        => await _sender.Send(query, ct);
}
