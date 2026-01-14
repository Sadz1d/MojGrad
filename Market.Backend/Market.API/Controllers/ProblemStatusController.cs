
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
using Market.Application.Modules.Reports.ProblemStatus.Commands.Update;
using Market.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Market.API.Controllers;

[ApiController]
[Route("api/reports/statuses")]
public sealed class ProblemStatusesController : ControllerBase
{
    private readonly ISender sender;
    private readonly IAppDbContext context;
    public ProblemStatusesController(ISender sender, IAppDbContext context)
    {
        this.sender = sender;
        this.context = context; // DODAJTE OVO
    }

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

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
       int id, [FromBody] UpdateProblemStatusCommand command, CancellationToken ct)
    {
        command.Id = id;
        await sender.Send(command, ct);
        return NoContent();
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await sender.Send(new DeleteProblemStatusCommand { Id = id }, ct);
        return NoContent();
    }
    [HttpGet("dropdown")]
    [ProducesResponseType(typeof(List<StatusDropdownDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDropdownOptions(CancellationToken ct)
    {
        var statuses = await context.ProblemStatuses
            .Where(s => !s.IsDeleted) // Samo aktivne
            .OrderBy(s => s.Name)
            .Select(s => new StatusDropdownDto
            {
                Id = s.Id,
                Name = s.Name
            })
            .ToListAsync(ct);

        return Ok(statuses);
    }
}

// DODAJTE OVAJ DTO
public class StatusDropdownDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}