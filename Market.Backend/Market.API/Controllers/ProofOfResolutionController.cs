

//using Market.Application.Modules.Reports.ProofOfResolution.Queries.GetById;
//using Market.Application.Modules.Reports.ProofOfResolution.Commands.Create;
//using Market.Application.Modules.Reports.ProofOfResolution.Commands.Update;
//using Market.Application.Modules.Reports.ProofOfResolution.Commands.Delete;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Market.Application.Modules.Reports.ProofOfResolution.Queries.List;
using Market.Application.Modules.Reports.ProofsOfResolution.Queries.List;
using Market.Application.Modules.Reports.ProofOfResolution.Queries.GetById;
using Market.Application.Abstractions;
using Market.Application.Common.Exceptions;
using Market.Application.Modules.Reports.ProofOfResolution.Commands.Create;
using Market.Domain.Entities.Reports;
using Market.Application.Modules.Reports.ProofOfResolution.Commands.Delete;
using Market.Application.Modules.Reports.ProofOfResolution.Commands.Update;

namespace Market.API.Controllers;

[ApiController]
[Route("api/reports/proofs-of-resolution")]
public sealed class ProofOfResolutionController : ControllerBase
{
    private readonly ISender sender;
    public ProofOfResolutionController(ISender sender) => this.sender = sender;

    [HttpGet]
    public async Task<PageResult<ListProofOfResolutionQueryDto>> List(
        [FromQuery] ListProofOfResolutionQuery query, CancellationToken ct)
        => await sender.Send(query, ct);

    [HttpGet("{id:int}")]
    public async Task<GetProofOfResolutionByIdQueryDto> GetById(int id, CancellationToken ct)
        => await sender.Send(new GetProofOfResolutionByIdQuery { Id = id }, ct);


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProofOfResolutionCommand command, CancellationToken ct)
    {
        var id = await sender.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProofOfResolutionCommand command, CancellationToken ct)
    {
        command.Id = id;
        await sender.Send(command, ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await sender.Send(new DeleteProofOfResolutionCommand { Id = id }, ct);
        return NoContent();
    }


}