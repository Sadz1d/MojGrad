using Market.Application.Modules.Civic.Events.Commands.Create;
using Market.Application.Modules.Civic.Events.Queries.GetById;
using Market.Application.Modules.Events.EventCalendar.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

    // GET: /api/EventCalendar/list
    [HttpGet("list")]
    public async Task<IActionResult> List([FromQuery] ListEventCalendarQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    // POST: /api/EventCalendar
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEventCalendarCommand command, CancellationToken ct)
    {
        var id = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    // GET: /api/EventCalendar/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        
        var result = await _mediator.Send(new GetEventCalendarByIdQuery { Id = id }, ct);
        return Ok(result);

    }
}
