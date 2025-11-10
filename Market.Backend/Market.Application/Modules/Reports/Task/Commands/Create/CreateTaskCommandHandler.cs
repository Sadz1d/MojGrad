using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Reports;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Reports.Tasks.Commands.Create;

public sealed class CreateTaskCommandHandler
    : IRequestHandler<CreateTaskCommand, int>
{
    private readonly IAppDbContext _ctx;
    public CreateTaskCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<int> Handle(CreateTaskCommand request, CancellationToken ct)
    {
        // ✅ provjeri postoji li Report
        var report = await _ctx.ProblemReports
            .FirstOrDefaultAsync(r => r.Id == request.ReportId, ct);
        if (report == null)
            throw new MarketNotFoundException($"Report with ID {request.ReportId} not found.");

        // ✅ provjeri postoji li Worker (korisnik)
        var worker = await _ctx.Users
            .FirstOrDefaultAsync(u => u.Id == request.WorkerId, ct);
        if (worker == null)
            throw new MarketNotFoundException($"Worker with ID {request.WorkerId} not found.");

        // ✅ provjeri da li već postoji aktivan task za isti report i korisnika
        var duplicate = await _ctx.Tasks
            .AnyAsync(t => t.ReportId == request.ReportId && t.WorkerId == request.WorkerId, ct);
        if (duplicate)
            throw new MarketConflictException("A task for this report and worker already exists.");

        // ✅ kreiraj entitet
        var entity = new TaskEntity
        {
            ReportId = request.ReportId,
            WorkerId = request.WorkerId,
            AssignmentDate = request.AssignmentDate ?? DateTime.UtcNow,
            Deadline = request.Deadline,
            TaskStatus = request.TaskStatus.Trim()
        };

        _ctx.Tasks.Add(entity);
        await _ctx.SaveChangesAsync(ct);

        return entity.Id;
    }
}
