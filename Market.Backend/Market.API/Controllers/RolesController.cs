using Market.Application.Modules.Identity.Roles.Commands.Create;
using Market.Application.Modules.Identity.Roles.Commands.Delete;
using Market.Application.Modules.Identity.Roles.Commands.Update;
using Market.Application.Modules.Identity.Roles.Queries.GetById;
using Market.Application.Modules.Identity.Roles.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers.Identity;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("list")]
    public async Task<IActionResult> List()
    {
        var result = await _mediator.Send(new ListRolesQuery());
        return Ok(result);
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetRoleByIdQuery { Id = id });
        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRoleCommand command)
    {
        command.Id = id; // ID dolazi iz rute
        await _mediator.Send(command);
        return NoContent();
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteRoleCommand { Id = id });
        return NoContent();
    }

}
