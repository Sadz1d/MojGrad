using Market.Application.Modules.Identity.RefreshTokens.Commands.Create;
using Market.Application.Modules.Identity.RefreshTokens.Queries.GetById;
using Market.Application.Modules.Identity.RefreshTokens.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers.Identity;

[ApiController]
[Route("api/[controller]")]
public class RefreshTokensController : ControllerBase
{
    private readonly IMediator _mediator;

    public RefreshTokensController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("list")]
    public async Task<IActionResult> List([FromQuery] ListRefreshTokensQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetRefreshTokenByIdQuery { Id = id });
        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRefreshTokenCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

}
