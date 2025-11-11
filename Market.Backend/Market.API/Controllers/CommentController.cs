
using Market.Application.Modules.Reports.Comment.Queries.List;
//using Market.Application.Modules.Reports.Comments.Queries.GetById;
//using Market.Application.Modules.Reports.Comments.Commands.Create;
//using Market.Application.Modules.Reports.Comments.Commands.Update;
//using Market.Application.Modules.Reports.Comments.Commands.Delete;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers;

[ApiController]
[Route("api/reports/comments")]
public sealed class CommentsController : ControllerBase
{
    private readonly ISender sender;
    public CommentsController(ISender sender) => this.sender = sender;

    [HttpGet]
    public async Task<PageResult<ListCommentQueryDto>> List([FromQuery] ListCommentQuery query, CancellationToken ct)
        => await sender.Send(query, ct);


}