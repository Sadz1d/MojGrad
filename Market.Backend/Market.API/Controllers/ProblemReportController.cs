
using Market.Application.Modules.Reports.ProblemReport.Commands.Create;
using Market.Application.Modules.Reports.ProblemReport.Commands.Delete;
using Market.Application.Modules.Reports.ProblemReport.Commands.Update;
using Market.Application.Modules.Reports.ProblemReport.Queries.GetById;
using Market.Application.Modules.Reports.ProblemReport.Queries.List;
//using Market.Application.Modules.Reports.ProblemReport.Queries.GetById;
//using Market.Application.Modules.Reports.ProblemReport.Commands.Create;
//using Market.Application.Modules.Reports.ProblemReport.Commands.Update;
//using Market.Application.Modules.Reports.ProblemReport.Commands.Delete;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers;

[ApiController]
[Route("api/reports/problem-reports")]
public sealed class ProblemReportsController : ControllerBase
{
    private readonly ISender sender;
    public ProblemReportsController(ISender sender) => this.sender = sender;

    [HttpGet]
    public async Task<PageResult<ListProblemReportQueryDto>> List(
        [FromQuery] ListProblemReportQuery query, CancellationToken ct)
        => await sender.Send(query, ct);

    [HttpGet("{id:int}")]
    public async Task<GetProblemReportByIdQueryDto> GetById(int id, CancellationToken ct)
    => await sender.Send(new GetProblemReportByIdQuery { Id = id }, ct);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProblemReportCommand command, CancellationToken ct)
    {
        var id = await sender.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProblemReportCommand command, CancellationToken ct)
    {
        command.Id = id;
        await sender.Send(command, ct);
        return NoContent();
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await sender.Send(new DeleteProblemReportCommand { Id = id }, ct);
        return NoContent();
    }
}