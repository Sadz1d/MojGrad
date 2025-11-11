using MediatR;
using Microsoft.AspNetCore.Mvc;
using Market.Application.Modules.Identity.Users.Commands.Create;
using Market.Application.Modules.Identity.Users.Commands.Update;
using Market.Application.Modules.Identity.Users.Commands.Delete;
using Market.Application.Modules.Identity.Users.Queries.List;
using Market.Application.Modules.Identity.Users.Queries.GetById;

namespace Market.API.Controllers.Identity;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/users/list
    [HttpGet("list")]
    public async Task<IActionResult> List([FromQuery] ListMarketUsersQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    // GET: api/users/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetMarketUserByIdQuery { Id = id });
        return Ok(result);
    }

    // POST: api/users
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMarketUserCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    // PUT: api/users/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMarketUserCommand command)
    {
        // Kreiramo novu instancu komande s ID-em iz rute
        var updatedCommand = new UpdateMarketUserCommand
        {
            Id = id,
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            PasswordHash = command.PasswordHash,
            IsAdmin = command.IsAdmin,
            IsManager = command.IsManager,
            IsEmployee = command.IsEmployee,
            IsEnabled = command.IsEnabled
        };

        await _mediator.Send(updatedCommand);
        return NoContent();
    }

    // DELETE: api/users/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteMarketUserCommand { Id = id });
        return NoContent();
    }
}
