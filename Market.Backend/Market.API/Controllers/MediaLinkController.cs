
using Market.Application.Modules.Media.MediaLink.Commands.Create;
using Market.Application.Modules.Media.MediaLink.Commands.Delete;
using Market.Application.Modules.Media.MediaLink.Queries.GetById;
using Market.Application.Modules.Media.MediaLink.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

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

    // GET /api/media/links/{id}
    [HttpGet("{id:int}")]
    public async Task<GetMediaLinkByIdQueryDto> GetById(int id, CancellationToken ct)
        => await _sender.Send(new GetMediaLinkByIdQuery { Id = id }, ct);

    // POST /api/media/links
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMediaLinkCommand command, CancellationToken ct)
    {
        var id = await _sender.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    // DELETE /api/media/links/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _sender.Send(new DeleteMediaLinkCommand { Id = id }, ct);
        return NoContent();
    }


}

