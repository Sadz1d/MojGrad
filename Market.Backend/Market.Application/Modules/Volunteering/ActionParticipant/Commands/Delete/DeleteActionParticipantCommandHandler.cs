using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Volunteering.ActionParticipants.Commands.Delete;

public sealed class DeleteActionParticipantCommandHandler
    : IRequestHandler<DeleteActionParticipantCommand, Unit>
{
    private readonly IAppDbContext _ctx;
    public DeleteActionParticipantCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(DeleteActionParticipantCommand request, CancellationToken ct)
    {
        var entity = await _ctx.ActionParticipants
            .FirstOrDefaultAsync(p => p.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"ActionParticipant with Id {request.Id} not found.");

        _ctx.ActionParticipants.Remove(entity);
        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
