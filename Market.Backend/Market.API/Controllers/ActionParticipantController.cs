using Market.Application.Modules.Volunteering.ActionParticipants.Commands.Create;
using Market.Application.Modules.Volunteering.ActionParticipants.Queries.GetById;
using Market.Application.Modules.Volunteering.ActionParticipants.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers;

[ApiController]
[Route("api/volunteering/action-participants")]
public sealed class ActionParticipantsController : ControllerBase
{
    private readonly ISender _sender;
    public ActionParticipantsController(ISender sender) => _sender = sender;

    [HttpGet]
    public async Task<PageResult<ListActionParticipantsQueryDto>> List(
        [FromQuery] ListActionParticipantsQuery query,
        CancellationToken ct)
        => await _sender.Send(query, ct);

    [HttpGet("{id:int}")]
    public async Task<GetActionParticipantByIdQueryDto> GetById(int id, CancellationToken ct)
    => await _sender.Send(new GetActionParticipantByIdQuery { Id = id }, ct);

    [HttpPost]
    public async Task<int> Create([FromBody] CreateActionParticipantCommand command, CancellationToken ct)
    => await _sender.Send(command, ct);


}
