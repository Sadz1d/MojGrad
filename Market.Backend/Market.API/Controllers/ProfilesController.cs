using MediatR;
using Microsoft.AspNetCore.Mvc;
using Market.Application.Modules.Identity.Profiles.Queries.List;

namespace Market.API.Controllers.Identity;

[ApiController]
[Route("api/[controller]")]
public class ProfilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfilesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("list")]
    public async Task<IActionResult> List([FromQuery] ListProfilesQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
