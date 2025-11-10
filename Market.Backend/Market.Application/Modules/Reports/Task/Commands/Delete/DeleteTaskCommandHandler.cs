using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Reports;

namespace Market.Application.Modules.Reports.Tasks.Commands.Delete;

public sealed class DeleteTaskCommandHandler
    : IRequestHandler<DeleteTaskCommand, Unit>
{
    private readonly IAppDbContext _ctx;

    public DeleteTaskCommandHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken ct)
    {
        var task = await _ctx.Tasks
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (task == null)
            throw new MarketNotFoundException($"Task with Id {request.Id} not found.");

        _ctx.Tasks.Remove(task);
        await _ctx.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
