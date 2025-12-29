using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Volunteering.ActionParticipants.Commands.Update;

public sealed class UpdateActionParticipantCommandHandler
    : IRequestHandler<UpdateActionParticipantCommand, Unit>
{
    private readonly IAppDbContext _ctx;
    public UpdateActionParticipantCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(UpdateActionParticipantCommand request, CancellationToken ct)
    {
        var entity = await _ctx.ActionParticipants
            .Include(p => p.Action)
            .FirstOrDefaultAsync(p => p.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"ActionParticipant (Id={request.Id}) not found.");

        // pripremi ciljna polja (ako nisu poslata, zadrži postojeće)
        var newActionId = request.ActionId ?? entity.ActionId;
        var newUserId = request.UserId ?? entity.UserId;

        if (!newActionId.HasValue || !newUserId.HasValue)
            throw new ArgumentException("ActionId and UserId cannot be null.");

        // ako se mijenja akcija → provjeri da postoji
        if (request.ActionId.HasValue && request.ActionId.Value != entity.ActionId)
        {
            var action = await _ctx.VolunteerActions
                .FirstOrDefaultAsync(a => a.Id == request.ActionId.Value, ct);
            if (action is null)
                throw new MarketNotFoundException($"Volunteer action with ID {request.ActionId} not found.");

            // zabrani update na prošlu akciju
            if (action.EventDate < DateTime.UtcNow)
                throw new MarketConflictException("Cannot assign participant to an action that already passed.");

            // kapacitet (broji druge učesnike)
            var count = await _ctx.ActionParticipants
                .CountAsync(p => p.ActionId == action.Id && p.Id != entity.Id, ct);
            if (count >= action.MaxParticipants)
                throw new MarketConflictException("Action has reached maximum number of participants.");

            entity.ActionId = action.Id;
        }

        // ako se mijenja user → provjeri da postoji
        if (request.UserId.HasValue && request.UserId.Value != entity.UserId)
        {
            var userExists = await _ctx.Users.AnyAsync(u => u.Id == request.UserId.Value, ct);
            if (!userExists)
                throw new MarketNotFoundException($"User with ID {request.UserId} not found.");

            entity.UserId = request.UserId.Value;
        }

        // zabrani duplikat (isti user na istoj akciji, drugi zapis)
        var duplicate = await _ctx.ActionParticipants.AnyAsync(p =>
                p.Id != entity.Id &&
                p.ActionId == entity.ActionId &&
                p.UserId == entity.UserId, ct);
        if (duplicate)
            throw new MarketConflictException("This user is already registered for the selected action.");

        // promjena datuma (opcionalno)
        if (request.RegistrationDate.HasValue)
            entity.RegistrationDate = request.RegistrationDate.Value;

        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
