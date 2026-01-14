
using Market.Application.Abstractions;
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
using Microsoft.EntityFrameworkCore;

namespace Market.API.Controllers;

[ApiController]
[Route("api/reports/categories")]
public sealed class ProblemCategoriesController : ControllerBase
{
    private readonly ISender sender;
    private readonly IAppDbContext context;
    public ProblemCategoriesController(ISender sender, IAppDbContext context)
    {
        this.sender = sender;
        this.context = context; // DODAJTE OVO
    }

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
    [HttpGet("dropdown")]
    [ProducesResponseType(typeof(List<CategoryDropdownDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDropdownOptions(CancellationToken ct)
    {
        var categories = await context.ProblemCategories
            .Where(c => !c.IsDeleted && c.IsEnabled) // Samo aktivne
            .OrderBy(c => c.Name)
            .Select(c => new CategoryDropdownDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            })
            .ToListAsync(ct);

        return Ok(categories);
    }
    public class CategoryDropdownDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}