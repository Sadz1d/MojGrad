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




}
