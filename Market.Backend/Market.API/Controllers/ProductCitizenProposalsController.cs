using MediatR;
using Microsoft.AspNetCore.Mvc;
using Market.Application.Modules.Civic.CitizenProposals.Queries.List;
using Market.Application.Modules.Civic.CitizenProposals.Queries.GetById;

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

}
