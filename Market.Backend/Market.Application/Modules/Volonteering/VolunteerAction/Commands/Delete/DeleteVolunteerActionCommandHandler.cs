using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Volunteering.VolunteerActions.Commands.Delete;

public sealed class DeleteVolunteerActionCommandHandler
    : IRequestHandler<DeleteVolunteerActionCommand, Unit>
{
    private readonly IAppDbContext _ctx;
    public DeleteVolunteerActionCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(DeleteVolunteerActionCommand request, CancellationToken ct)
    {
        var entity = await _ctx.VolunteerActions
            .FirstOrDefaultAsync(a => a.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"VolunteerAction with Id {request.Id} not found.");

        // Blokiraj brisanje ako postoje učesnici (spriječi orphan podatke)
        var hasParticipants = await _ctx.ActionParticipants
            .AnyAsync(p => p.ActionId == entity.Id, ct);
        if (hasParticipants)
            throw new MarketConflictException("Cannot delete this action because it has registered participants.");

        _ctx.VolunteerActions.Remove(entity);
        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
