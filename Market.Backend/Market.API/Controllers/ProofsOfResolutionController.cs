

//using Market.Application.Modules.Reports.ProofOfResolution.Queries.GetById;
//using Market.Application.Modules.Reports.ProofOfResolution.Commands.Create;
//using Market.Application.Modules.Reports.ProofOfResolution.Commands.Update;
//using Market.Application.Modules.Reports.ProofOfResolution.Commands.Delete;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Market.Application.Modules.Reports.ProofOfResolution.Queries.List;
using Market.Application.Modules.Reports.ProofsOfResolution.Queries.List;

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
}