//using Market.Application.Abstractions.Messaging;
using Market.Application.Modules.Media.Commands.Create;
using Market.Application.Modules.Media.Queries.GetById;
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

    [HttpGet("{id:int}")]
    public async Task<GetMediaAttachmentByIdQueryDto> GetById(
       [FromRoute] int id,
       CancellationToken ct)
    {
        var query = new GetMediaAttachmentByIdQuery { Id = id };
        var result = await _sender.Send(query, ct);
        return result;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
       [FromBody] CreateMediaAttachmentCommand command,
       CancellationToken ct)
    {
        var id = await _sender.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }
}
