
using Market.Application.Modules.Reports.ProblemCategory.Commands.Create;
using Market.Application.Modules.Reports.ProblemCategory.Queries.GetById;
using Market.Application.Modules.Reports.ProblemCategory.Queries.List;
//using Market.Application.Modules.Reports.ProblemCategory.Queries.GetById;
//using Market.Application.Modules.Reports.ProblemCategory.Commands.Create;
//using Market.Application.Modules.Reports.ProblemCategory.Commands.Update;
//using Market.Application.Modules.Reports.ProblemCategory.Commands.Delete;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers;

[ApiController]
[Route("api/reports/categories")]
public sealed class ProblemCategoriesController : ControllerBase
{
    private readonly ISender sender;
    public ProblemCategoriesController(ISender sender) => this.sender = sender;

    [HttpGet]
    public async Task<PageResult<ListProblemCategoryQueryDto>> List([FromQuery] ListProblemCategoryQuery query, CancellationToken ct)
        => await sender.Send(query, ct);

    [HttpGet("{id:int}")]
    public async Task<GetProblemCategoryByIdQueryDto> GetById(int id, CancellationToken ct)
      => await sender.Send(new GetProblemCategoryByIdQuery { Id = id }, ct);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProblemCategoryCommand command, CancellationToken ct)
    {
        var id = await sender.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }
}