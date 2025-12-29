using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Volunteering;

namespace Market.Application.Modules.Volunteering.ActionParticipants.Commands.Create;

public sealed class CreateActionParticipantCommandHandler
    : IRequestHandler<CreateActionParticipantCommand, int>
{
    private readonly IAppDbContext _ctx;
    public CreateActionParticipantCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<int> Handle(CreateActionParticipantCommand request, CancellationToken ct)
    {
        // 1) Akcija postoji?
        var action = await _ctx.VolunteerActions
            .FirstOrDefaultAsync(a => a.Id == request.ActionId, ct);
        if (action is null)
            throw new MarketNotFoundException($"Volunteer action with ID {request.ActionId} not found.");

        // 2) User postoji?
        var userExists = await _ctx.Users.AnyAsync(u => u.Id == request.UserId, ct);
        if (!userExists)
            throw new MarketNotFoundException($"User with ID {request.UserId} not found.");

        // 3) Već prijavljen?
        var alreadyJoined = await _ctx.ActionParticipants
            .AnyAsync(p => p.ActionId == request.ActionId && p.UserId == request.UserId, ct);
        if (alreadyJoined)
            throw new MarketConflictException("User is already registered for this action.");

        // 4) Kapacitet pun?
        var currentCount = await _ctx.ActionParticipants
            .CountAsync(p => p.ActionId == request.ActionId, ct);
        if (currentCount >= action.MaxParticipants)
            throw new MarketConflictException("Action has reached maximum number of participants.");

        // 5) (opcija) Zabrani prijavu nakon datuma održavanja
        if (action.EventDate < DateTime.UtcNow)
            throw new MarketConflictException("Cannot register for an action that has already passed.");

        // 6) Kreiraj zapis
        var entity = new ActionParticipantEntity
        {
            ActionId = request.ActionId,
            UserId = request.UserId,
            RegistrationDate = request.RegistrationDate ?? DateTime.UtcNow
        };

        _ctx.ActionParticipants.Add(entity);
        await _ctx.SaveChangesAsync(ct);
        return entity.Id;
    }
}
