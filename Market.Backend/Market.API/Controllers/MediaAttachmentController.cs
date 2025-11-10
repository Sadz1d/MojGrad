//using Market.Application.Abstractions.Messaging;
using Market.Application.Modules.Media.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers;

[ApiController]
[Route("api/media/attachments")]
public sealed class MediaAttachmentController : ControllerBase
{
    private readonly ISender _sender;
    public MediaAttachmentController(ISender sender) => _sender = sender;

    // GET /api/media/attachments?searchMime=image&uploaderId=12&page=1&pageSize=20
    [HttpGet]
    public async Task<PageResult<ListMediaAttachmentsQueryDto>> List(
        [FromQuery] ListMediaAttachmentsQuery query,
        CancellationToken ct)
    {
        var result = await _sender.Send(query, ct);
        return result;
    }
}
