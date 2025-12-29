using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Volunteering;

namespace Market.Application.Modules.Volunteering.VolunteerActions.Commands.Update;

public sealed class UpdateVolunteerActionCommandHandler
    : IRequestHandler<UpdateVolunteerActionCommand, Unit>
{
    private readonly IAppDbContext _ctx;
    public UpdateVolunteerActionCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(UpdateVolunteerActionCommand request, CancellationToken ct)
    {
        var entity = await _ctx.VolunteerActions
            .FirstOrDefaultAsync(a => a.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"VolunteerAction (Id={request.Id}) not found.");

        // broj već prijavljenih (za provjeru MaxParticipants)
        var currentParticipants = await _ctx.ActionParticipants
            .CountAsync(p => p.ActionId == entity.Id, ct);

        // Organizer
        if (request.VolunteerId.HasValue)
        {
            if (request.VolunteerId.Value <= 0)
                throw new MarketConflictException("VolunteerId must be greater than 0.");

            var exists = await _ctx.Users.AnyAsync(u => u.Id == request.VolunteerId.Value, ct);
            if (!exists)
                throw new MarketNotFoundException($"Organizer (UserId={request.VolunteerId}) not found.");

            entity.VolunteerId = request.VolunteerId.Value;
        }

        // Name
        if (request.Name is not null)
        {
            var name = request.Name.Trim();
            if (string.IsNullOrWhiteSpace(name))
                throw new MarketConflictException("Name cannot be empty.");
            if (name.Length > VolunteerActionEntity.Constraints.NameMaxLength)
                throw new MarketConflictException($"Name exceeds {VolunteerActionEntity.Constraints.NameMaxLength} characters.");
            entity.Name = name;
        }

        // Description
        if (request.Description is not null)
        {
            var desc = request.Description.Trim();
            if (desc.Length > VolunteerActionEntity.Constraints.DescriptionMaxLength)
                throw new MarketConflictException($"Description exceeds {VolunteerActionEntity.Constraints.DescriptionMaxLength} characters.");
            entity.Description = desc;
        }

        // Location
        if (request.Location is not null)
        {
            var loc = request.Location.Trim();
            if (loc.Length > VolunteerActionEntity.Constraints.LocationMaxLength)
                throw new MarketConflictException($"Location exceeds {VolunteerActionEntity.Constraints.LocationMaxLength} characters.");
            entity.Location = loc;
        }

        // EventDate
        if (request.EventDate.HasValue)
        {
            var date = request.EventDate.Value;
            if (date < DateTime.UtcNow.AddMinutes(-1))
                throw new MarketConflictException("EventDate cannot be in the past.");
            entity.EventDate = date;
        }

        // MaxParticipants
        if (request.MaxParticipants.HasValue)
        {
            if (request.MaxParticipants.Value <= 0)
                throw new MarketConflictException("MaxParticipants must be greater than 0.");
            if (request.MaxParticipants.Value < currentParticipants)
                throw new MarketConflictException($"MaxParticipants ({request.MaxParticipants.Value}) cannot be less than current registrations ({currentParticipants}).");

            entity.MaxParticipants = request.MaxParticipants.Value;
        }

        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
