using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Reports;

namespace Market.Application.Modules.Reports.Tasks.Queries.List;

public sealed class ListTasksQueryHandler
    : IRequestHandler<ListTasksQuery, PageResult<ListTasksQueryDto>>
{
    private readonly IAppDbContext _ctx;

    public ListTasksQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<PageResult<ListTasksQueryDto>> Handle(ListTasksQuery request, CancellationToken ct)
    {
        IQueryable<TaskEntity> q = _ctx.Tasks
            .AsNoTracking()
            .Include(t => t.Worker)
            .Include(t => t.Report);

        // 🔍 Pretraga po statusu, imenu radnika, nazivu izvještaja
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            q = q.Where(t =>
                t.TaskStatus.ToLower().Contains(term) ||
                (t.Worker.FirstName + " " + t.Worker.LastName).ToLower().Contains(term) ||
                t.Report.Title.ToLower().Contains(term));
        }

        // ✅ Filtriranje po završenim zadacima
        if (request.OnlyCompleted.HasValue && request.OnlyCompleted.Value)
        {
            q = q.Where(t => t.TaskStatus.ToLower() == "completed");
        }

        // 📄 Filtriranje po izvještaju
        if (request.ReportId.HasValue)
        {
            q = q.Where(t => t.ReportId == request.ReportId.Value);
        }

        var projected = q
            .OrderByDescending(t => t.AssignmentDate)
            .Select(t => new ListTasksQueryDto
            {
                Id = t.Id,
                ReportTitle = t.Report.Title,
                WorkerName = (t.Worker.FirstName + " " + t.Worker.LastName).Trim(),
                AssignmentDate = t.AssignmentDate,
                Deadline = t.Deadline,
                TaskStatus = t.TaskStatus
            });

        return await PageResult<ListTasksQueryDto>
            .FromQueryableAsync(projected, request.Paging, ct);
    }
}
