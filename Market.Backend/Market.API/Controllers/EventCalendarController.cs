using Market.Application.Modules.Civic.Events.Commands.Create;
using Market.Application.Modules.Civic.Events.Queries.GetById;
using Market.Application.Modules.Events.EventCalendar.Commands.Delete;
using Market.Application.Modules.Events.EventCalendar.Commands.Update;
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

    [HttpGet("list")]
    public async Task<IActionResult> List([FromQuery] ListEventCalendarQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetEventCalendarByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateEventCalendarCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEventCalendarCommand command)
    {
        command.Id = id; // ID dolazi iz rute
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteEventCalendarCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}
