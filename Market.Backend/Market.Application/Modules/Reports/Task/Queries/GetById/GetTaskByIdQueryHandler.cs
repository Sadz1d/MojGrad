using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Reports;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Reports.Tasks.Queries.GetById;

public sealed class GetTaskByIdQueryHandler
    : IRequestHandler<GetTaskByIdQuery, GetTaskByIdQueryDto>
{
    private readonly IAppDbContext _ctx;
    public GetTaskByIdQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<GetTaskByIdQueryDto> Handle(
        GetTaskByIdQuery request, CancellationToken ct)
    {
        var task = await _ctx.Tasks
            .AsNoTracking()
            .Include(t => t.Worker)
            .Include(t => t.Report)
            .Where(t => t.Id == request.Id)
            .Select(t => new GetTaskByIdQueryDto
            {
                Id = t.Id,
                ReportTitle = t.Report.Title,
                WorkerName = (t.Worker.FirstName + " " + t.Worker.LastName).Trim(),
                AssignmentDate = t.AssignmentDate,
                Deadline = t.Deadline,
                TaskStatus = t.TaskStatus
            })
            .FirstOrDefaultAsync(ct);

        if (task == null)
            throw new MarketNotFoundException($"Task with Id {request.Id} not found.");

        return task;
    }
}
