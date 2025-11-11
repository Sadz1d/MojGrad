// Market.API/Controllers/AssignedRewardsController.cs
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Market.Application.Modules.Rewards.AssignedRewards.Queries.List;
using Market.Application.Modules.Rewards.AssignedRewards.Queries.GetById;
using Market.Application.Modules.Rewards.AssignedRewards.Commands.Create;
using Market.Application.Modules.Rewards.AssignedRewards.Commands.Delete;

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

   // GET /api/rewards/assigned/{id}
    [HttpGet("{id:int}")]
    public async Task<GetAssignedRewardByIdQueryDto> GetById(int id, CancellationToken ct)
        => await _sender.Send(new GetAssignedRewardByIdQuery { Id = id }, ct);

    [HttpPost]
    public async Task<int> Create([FromBody] CreateAssignedRewardCommand command, CancellationToken ct)
    {
        var id = await _sender.Send(command, ct);
        return id; // ili return CreatedAtAction(...), ako želiš lokaciju
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _sender.Send(new DeleteAssignedRewardCommand { Id = id }, ct);
        return NoContent();
    }
}
