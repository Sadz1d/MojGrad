using Market.Application.Abstractions;
using Market.Application.Modules.Identity.Users.Commands.Create;
using Market.Application.Modules.Identity.Users.Commands.Delete;
using Market.Application.Modules.Identity.Users.Commands.Update;
using Market.Application.Modules.Identity.Users.Queries.GetById;
using Market.Application.Modules.Identity.Users.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Market.API.Controllers.Identity;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAppDbContext context;

    public UsersController(IMediator mediator, IAppDbContext context)
    {
        _mediator = mediator;
        this.context = context;
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
    [HttpGet("dropdown")]
    [ProducesResponseType(typeof(List<UserDropdownDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDropdownOptions(CancellationToken ct)
    {
        var statuses = await context.Users
            .Where(s => !s.IsDeleted) 
            .OrderBy(s => s.FirstName)
            .Select(s => new UserDropdownDto
            {
                Id = s.Id,
                Name = s.FirstName
            })
            .ToListAsync(ct);

        return Ok(statuses);
    }
}
public class UserDropdownDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
