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

}
