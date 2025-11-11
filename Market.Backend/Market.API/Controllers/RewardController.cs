using Market.Application.Modules.Rewards.Reward.Commands.Create;
using Market.Application.Modules.Rewards.Reward.Commands.Delete;
using Market.Application.Modules.Rewards.Reward.Commands.Update;
using Market.Application.Modules.Rewards.Reward.Queries.GetById;
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

    [HttpGet("{id:int}")]
    public async Task<GetRewardByIdQueryDto> GetById(int id, CancellationToken ct)
    => await _sender.Send(new GetRewardByIdQuery { Id = id }, ct);

    [HttpPost]
    public async Task<int> Create([FromBody] CreateRewardCommand command, CancellationToken ct)
    => await _sender.Send(command, ct);

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _sender.Send(new DeleteRewardCommand { Id = id }, ct);
        return NoContent();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRewardCommand command, CancellationToken ct)
    {
        command.Id = id;
        await _sender.Send(command, ct);
        return NoContent();
    }
}
