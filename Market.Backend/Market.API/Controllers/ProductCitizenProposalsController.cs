using Market.Application.Modules.Civic.CitizenProposals.Commands.Create;
using Market.Application.Modules.Civic.CitizenProposals.Commands.Delete;
using Market.Application.Modules.Civic.CitizenProposals.Commands.Update;
using Market.Application.Modules.Civic.CitizenProposals.Queries.GetById;
using Market.Application.Modules.Civic.CitizenProposals.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers;

[ApiController]
[Route("api/civic/citizen-proposals")]
public sealed class CitizenProposalsController : ControllerBase
{
    private readonly ISender sender;
    public CitizenProposalsController(ISender sender) => this.sender = sender;

    // GET /api/civic/citizen-proposals?search=...&page=1&pageSize=20
    [HttpGet]
    public async Task<PageResult<ListCitizenProposalsQueryDto>> List(
        [FromQuery] ListCitizenProposalsQuery query,
        CancellationToken ct)
    {
        var result = await sender.Send(query, ct);
        return result;
    }



    [HttpGet("{id:int}")]
    public async Task<GetCitizenProposalByIdQueryDto> GetById(int id, CancellationToken ct)
    {
        var result = await sender.Send(new GetCitizenProposalByIdQuery { Id = id }, ct);
        return result;
    }
    // POST create (novo)
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCitizenProposalCommand command, CancellationToken ct)
    {
        var id = await sender.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await sender.Send(new DeleteCitizenProposalCommand { Id = id }, ct);
        return NoContent(); // 204
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id,
    [FromBody] UpdateCitizenProposalCommand command,
    CancellationToken ct)
    {
        // ID iz rute ima prednost (kao kod profa)
        command.Id = id;

        await sender.Send(command, ct);
        return NoContent(); // 204
    }
}
