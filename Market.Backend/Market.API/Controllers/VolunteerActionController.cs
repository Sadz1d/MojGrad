using Market.Application.Modules.Volunteering.VolunteerActions.Commands.Create;
using Market.Application.Modules.Volunteering.VolunteerActions.Commands.Delete;
using Market.Application.Modules.Volunteering.VolunteerActions.Commands.Update;
using Market.Application.Modules.Volunteering.VolunteerActions.Queries.GetById;
using Market.Application.Modules.Volunteering.VolunteerActions.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers;

[ApiController]
[Route("api/volunteering/actions")]
public sealed class VolunteerActionsController : ControllerBase
{
    private readonly ISender _sender;
    public VolunteerActionsController(ISender sender) => _sender = sender;

    // GET /api/volunteering/actions?search=&dateFrom=&dateTo=&onlyUpcoming=&onlyWithFreeSlots=&page=1&pageSize=20
    [HttpGet]
    public async Task<PageResult<ListVolunteerActionsQueryDto>> List(
        [FromQuery] ListVolunteerActionsQuery query,
        CancellationToken ct)
        => await _sender.Send(query, ct);

    [HttpGet("{id:int}")]
    public async Task<GetVolunteerActionByIdQueryDto> GetById(int id, CancellationToken ct)
    => await _sender.Send(new GetVolunteerActionByIdQuery { Id = id }, ct);

    [HttpPost]
    public async Task<int> Create([FromBody] CreateVolunteerActionCommand command, CancellationToken ct)
    => await _sender.Send(command, ct);

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _sender.Send(new DeleteVolunteerActionCommand { Id = id }, ct);
        return NoContent();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateVolunteerActionCommand command, CancellationToken ct)
    {
        command.Id = id;
        await _sender.Send(command, ct);
        return NoContent();
    }
}
