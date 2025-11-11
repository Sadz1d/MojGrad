using Market.Application.Modules.Identity.Profiles.Queries.GetById;
using Market.Application.Modules.Identity.Profiles.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetProfileByIdQuery { Id = id });
        return Ok(result);
    }
}
