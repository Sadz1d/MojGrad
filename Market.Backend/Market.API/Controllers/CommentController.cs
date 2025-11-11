
using Market.Application.Modules.Reports.Comment.Queries.GetById;
using Market.Application.Modules.Reports.Comment.Queries.List;
using Market.Application.Modules.Reports.Comments.Commands.Create;
using Market.Application.Modules.Reports.Comments.Commands.Delete;


//using Market.Application.Modules.Reports.Comments.Queries.GetById;
//using Market.Application.Modules.Reports.Comments.Commands.Create;
//using Market.Application.Modules.Reports.Comments.Commands.Update;
//using Market.Application.Modules.Reports.Comments.Commands.Delete;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers;

[ApiController]
[Route("api/reports/comments")]
public sealed class CommentController : ControllerBase
{
    private readonly ISender sender;
    public CommentController(ISender sender) => this.sender = sender;

    [HttpGet]
    public async Task<PageResult<ListCommentQueryDto>> List([FromQuery] ListCommentQuery query, CancellationToken ct)
        => await sender.Send(query, ct);

    [HttpGet("{id:int}")]
    public async Task<GetCommentByIdQueryDto> GetById(int id, CancellationToken ct)
        => await sender.Send(new GetCommentByIdQuery { Id = id }, ct);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCommentCommand command, CancellationToken ct)
    {
        var id = await sender.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await sender.Send(new DeleteCommentCommand { Id = id }, ct);
        return NoContent();
    }

}