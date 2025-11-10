using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Reports;

namespace Market.Application.Modules.Reports.Tasks.Commands.Update;

public sealed class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Unit>
{
    private readonly IAppDbContext _ctx;
    public UpdateTaskCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(UpdateTaskCommand request, CancellationToken ct)
    {
        var entity = await _ctx.Tasks
            .FirstOrDefaultAsync(t => t.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"Task (Id={request.Id}) not found.");

        // ažuriranje polja ako su poslata
        if (!string.IsNullOrWhiteSpace(request.TaskStatus))
            entity.TaskStatus = request.TaskStatus.Trim();

        if (request.Deadline.HasValue)
            entity.Deadline = request.Deadline.Value;

        if (request.Completed.HasValue)
            entity.TaskStatus = request.Completed.Value ? "Completed" : "In Progress";

        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
