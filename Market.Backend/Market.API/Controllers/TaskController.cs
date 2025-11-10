using Market.Application.Modules.Reports.Tasks.Commands.Create;
using Market.Application.Modules.Reports.Tasks.Commands.Delete;
using Market.Application.Modules.Reports.Tasks.Commands.Update;
using Market.Application.Modules.Reports.Tasks.Queries.GetById;
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
    // GET /api/reports/tasks/{id}
    [HttpGet("{id:int}")]
    public async Task<GetTaskByIdQueryDto> GetById(int id, CancellationToken ct)
    {
        var result = await sender.Send(new GetTaskByIdQuery { Id = id }, ct);
        return result;
    }

    // POST /api/reports/tasks
    [HttpPost]
    public async Task<int> Create([FromBody] CreateTaskCommand command, CancellationToken ct)
    {
        var id = await sender.Send(command, ct);
        return id;
    }

    // DELETE /api/reports/tasks/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await sender.Send(new DeleteTaskCommand { Id = id }, ct);
        return NoContent();
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskCommand command, CancellationToken ct)
    {
        command.Id = id;
        await sender.Send(command, ct);
        return NoContent();
    }


}
