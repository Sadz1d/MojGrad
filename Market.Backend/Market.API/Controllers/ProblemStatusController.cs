
using Market.Application.Modules.Reports.ProblemStatus.Queries.List;
//using Market.Application.Modules.Reports.ProblemStatus.Queries.GetById;
//using Market.Application.Modules.Reports.ProblemStatus.Commands.Create;
//using Market.Application.Modules.Reports.ProblemStatus.Commands.Update;
//using Market.Application.Modules.Reports.ProblemStatus.Commands.Delete;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Market.Application.Modules.Reports.ProblemStatus.Queries.List;
using Market.Application.Modules.Reports.ProblemStatus.Queries.GetById;

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
}