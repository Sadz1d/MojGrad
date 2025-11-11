using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Volunteering;

namespace Market.Application.Modules.Volunteering.VolunteerActions.Commands.Create;

public sealed class CreateVolunteerActionCommandHandler
    : IRequestHandler<CreateVolunteerActionCommand, int>
{
    private readonly IAppDbContext _ctx;
    public CreateVolunteerActionCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<int> Handle(CreateVolunteerActionCommand request, CancellationToken ct)
    {
        var name = request.Name.Trim();
        if (string.IsNullOrWhiteSpace(name))
            throw new MarketConflictException("Name is required.");

        // (opcija) jedinstveno ime i datum – ako to želite
        var duplicate = await _ctx.VolunteerActions
            .AnyAsync(a => a.Name == name && a.EventDate == request.EventDate, ct);
        if (duplicate)
            throw new MarketConflictException("A volunteer action with the same name and date already exists.");

        if (request.MaxParticipants <= 0)
            throw new MarketConflictException("MaxParticipants must be greater than 0.");

        // (opcija) ne dozvoli kreiranje u prošlosti
        if (request.EventDate < DateTime.UtcNow.AddMinutes(-1))
            throw new MarketConflictException("EventDate cannot be in the past.");

        // Ako je poslan organizator, provjeri da postoji
        if (request.VolunteerId.HasValue)
        {
            var organizerExists = await _ctx.Users.AnyAsync(u => u.Id == request.VolunteerId.Value, ct);
            if (!organizerExists)
                throw new MarketNotFoundException($"Organizer (UserId={request.VolunteerId}) not found.");
        }

        var entity = new VolunteerActionEntity
        {
            VolunteerId = request.VolunteerId,
            Name = name,
            Description = request.Description?.Trim(),
            Location = request.Location?.Trim(),
            EventDate = request.EventDate,
            MaxParticipants = request.MaxParticipants
        };

        _ctx.VolunteerActions.Add(entity);
        await _ctx.SaveChangesAsync(ct);
        return entity.Id;
    }
}
