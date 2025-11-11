using Market.Application.Modules.Identity.UserRoles.Commands.Create;
using Market.Application.Modules.Identity.UserRoles.Commands.Delete;
using Market.Application.Modules.Identity.UserRoles.Commands.Update;
using Market.Application.Modules.Identity.UserRoles.Queries.GetById;
using Market.Application.Modules.Identity.UserRoles.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers.Identity;

[ApiController]
[Route("api/[controller]")]
public class UserRolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserRolesController(IMediator mediator) => _mediator = mediator;

    [HttpGet("list")]
    public async Task<IActionResult> List([FromQuery] ListUserRolesQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetUserRoleByIdQuery { Id = id });
        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRoleCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRoleCommand command)
    {
        command = command with { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteUserRoleCommand(id));
        return NoContent();
    }
}
