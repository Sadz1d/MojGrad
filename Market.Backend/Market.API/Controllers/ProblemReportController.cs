
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
}