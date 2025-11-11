
using Market.Application.Modules.Reports.Rating.Queries.List;
//using Market.Application.Modules.Reports.Rating.Queries.GetById;
//using Market.Application.Modules.Reports.Rating.Commands.Create;
//using Market.Application.Modules.Reports.Rating.Commands.Update;
//using Market.Application.Modules.Reports.Rating.Commands.Delete;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Market.Application.Modules.Reports.Rating.Queries.List;
using Market.Application.Modules.Reports.Rating.Queries.GetById;
using Market.Application.Modules.Reports.Rating.Commands.Create;

namespace Market.API.Controllers;

[ApiController]
[Route("api/reports/ratings")]
public sealed class RatingsController : ControllerBase
{
    private readonly ISender sender;
    public RatingsController(ISender sender) => this.sender = sender;

    [HttpGet]
    public async Task<PageResult<ListRatingQueryDto>> List(
        [FromQuery] ListRatingQuery query, CancellationToken ct)
        => await sender.Send(query, ct);

    [HttpGet("{id:int}")]
    public async Task<GetRatingByIdQueryDto> GetById(int id, CancellationToken ct)
       => await sender.Send(new GetRatingByIdQuery { Id = id }, ct);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRatingCommand command, CancellationToken ct)
    {
        var id = await sender.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

}