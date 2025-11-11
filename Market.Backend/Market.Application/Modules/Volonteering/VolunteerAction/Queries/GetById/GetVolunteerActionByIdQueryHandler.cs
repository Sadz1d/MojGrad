using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Volunteering.VolunteerActions.Queries.GetById;

public sealed class GetVolunteerActionByIdQueryHandler
    : IRequestHandler<GetVolunteerActionByIdQuery, GetVolunteerActionByIdQueryDto>
{
    private readonly IAppDbContext _ctx;
    public GetVolunteerActionByIdQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<GetVolunteerActionByIdQueryDto> Handle(
        GetVolunteerActionByIdQuery request, CancellationToken ct)
    {
        var dto = await _ctx.VolunteerActions
            .AsNoTracking()
            .Include(a => a.Volunteer)
            .Where(a => a.Id == request.Id)
            .Select(a => new GetVolunteerActionByIdQueryDto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                Location = a.Location,
                EventDate = a.EventDate,
                MaxParticipants = a.MaxParticipants,
                OrganizerId = a.VolunteerId,
                OrganizerName = a.Volunteer != null
                    ? (a.Volunteer.FirstName + " " + a.Volunteer.LastName).Trim()
                    : null,
                ParticipantsCount = _ctx.ActionParticipants.Count(p => p.ActionId == a.Id)
            })
            .FirstOrDefaultAsync(ct);

        if (dto is null)
            throw new MarketNotFoundException($"VolunteerAction with Id {request.Id} not found.");

        return dto;
    }
}
