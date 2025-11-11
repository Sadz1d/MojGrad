using MediatR;
using Microsoft.AspNetCore.Mvc;
using Market.Application.Modules.Events.EventCalendar.Queries.List;

namespace Market.API.Controllers.Events;

[ApiController]
[Route("api/[controller]")]
public class EventCalendarController : ControllerBase
{
    private readonly IMediator _mediator;

    public EventCalendarController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("list")]
    public async Task<IActionResult> List([FromQuery] ListEventCalendarQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
