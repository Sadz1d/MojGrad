using MediatR;
using Microsoft.AspNetCore.Mvc;
using Market.Application.Modules.Identity.UserRoles.Queries.List;

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
}
