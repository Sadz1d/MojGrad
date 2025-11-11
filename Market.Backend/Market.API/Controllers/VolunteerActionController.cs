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
}
