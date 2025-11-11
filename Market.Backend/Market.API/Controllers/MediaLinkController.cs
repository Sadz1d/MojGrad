
using Market.Application.Modules.Media.MediaLink.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers;

[ApiController]
[Route("api/media/links")]
public sealed class MediaLinkController : ControllerBase
{
    private readonly ISender _sender;
    public MediaLinkController(ISender sender) => _sender = sender;

    // GET /api/media/links?entityType=ProblemReport&entityId=10&mediaId=5&page=1&pageSize=20
    [HttpGet]
    public async Task<PageResult<ListMediaLinkQueryDto>> List(
        [FromQuery] ListMediaLinkQuery query,
        CancellationToken ct)
    {
        var result = await _sender.Send(query, ct);
        return result;
    }
}

