
using Market.Application.Modules.Reports.ProblemStatus.Queries.List;
//using Market.Application.Modules.Reports.ProblemStatus.Queries.GetById;
//using Market.Application.Modules.Reports.ProblemStatus.Commands.Create;
//using Market.Application.Modules.Reports.ProblemStatus.Commands.Update;
//using Market.Application.Modules.Reports.ProblemStatus.Commands.Delete;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Market.Application.Modules.Reports.ProblemStatus.Queries.List;
using Market.Application.Modules.Reports.ProblemStatus.Queries.GetById;
using Market.Application.Modules.Reports.ProblemStatus.Commands.Create;
using Market.Application.Modules.Reports.ProblemStatus.Commands.Delete;

namespace Market.API.Controllers;

[ApiController]
[Route("api/reports/statuses")]
public sealed class ProblemStatusesController : ControllerBase
{
    private readonly ISender sender;
    public ProblemStatusesController(ISender sender) => this.sender = sender;

    [HttpGet]
    public async Task<PageResult<ListProblemStatusQueryDto>> List(
        [FromQuery] ListProblemStatusQuery query, CancellationToken ct)
        => await sender.Send(query, ct);

    [HttpGet("{id:int}")]
    public async Task<GetProblemStatusByIdQueryDto> GetById(int id, CancellationToken ct)
       => await sender.Send(new GetProblemStatusByIdQuery { Id = id }, ct);

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateProblemStatusCommand command, CancellationToken ct)
    {
        var id = await sender.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await sender.Send(new DeleteProblemStatusCommand { Id = id }, ct);
        return NoContent();
    }
}