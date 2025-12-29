
using Market.Application.Modules.Reports.ProblemCategories.Commands.Status.Disable;
using Market.Application.Modules.Reports.ProblemCategories.Commands.Status.Enable;
using Market.Application.Modules.Reports.ProblemCategory.Commands.Create;
using Market.Application.Modules.Reports.ProblemCategory.Commands.Delete;
using Market.Application.Modules.Reports.ProblemCategory.Commands.Update;
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

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProblemCategoryCommand command, CancellationToken ct)
    {
        command.Id = id;
        await sender.Send(command, ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await sender.Send(new DeleteProblemCategoryCommand { Id = id }, ct);
        return NoContent();
    }

    // ENABLE
    [HttpPut("{id:int}/enable")]
    public async Task<IActionResult> Enable(int id, CancellationToken ct)
    {
        await sender.Send(new EnableProblemCategoryCommand { Id = id }, ct);
        return NoContent();
    }

    // DISABLE
    [HttpPut("{id:int}/disable")]
    public async Task<IActionResult> Disable(int id, CancellationToken ct)
    {
        await sender.Send(new DisableProblemCategoryCommand { Id = id }, ct);
        return NoContent();
    }
}