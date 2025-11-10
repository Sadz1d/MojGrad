using Market.Application.Modules.Reports.Tasks.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers;

[ApiController]
[Route("api/reports/tasks")]
public sealed class TasksController : ControllerBase
{
    private readonly ISender sender;
    public TasksController(ISender sender) => this.sender = sender;

    // GET /api/reports/tasks?search=...&page=1&pageSize=20
    [HttpGet]
    public async Task<PageResult<ListTasksQueryDto>> List(
        [FromQuery] ListTasksQuery query,
        CancellationToken ct)
    {
        var result = await sender.Send(query, ct);
        return result;
    }
}
