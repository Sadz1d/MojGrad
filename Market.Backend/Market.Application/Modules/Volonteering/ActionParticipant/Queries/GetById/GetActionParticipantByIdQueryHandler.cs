using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Volunteering.ActionParticipants.Queries.GetById;

public sealed class GetActionParticipantByIdQueryHandler
    : IRequestHandler<GetActionParticipantByIdQuery, GetActionParticipantByIdQueryDto>
{
    private readonly IAppDbContext _ctx;
    public GetActionParticipantByIdQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<GetActionParticipantByIdQueryDto> Handle(
        GetActionParticipantByIdQuery request, CancellationToken ct)
    {
        var dto = await _ctx.ActionParticipants
            .AsNoTracking()
            .Include(p => p.User)
            .Include(p => p.Action)
            .Where(p => p.Id == request.Id)
            .Select(p => new GetActionParticipantByIdQueryDto
            {
                Id = p.Id,
                UserId = p.UserId,
                UserName = p.User != null
                    ? (p.User.FirstName + " " + p.User.LastName).Trim()
                    : null,
                ActionId = p.ActionId,
                ActionName = p.Action != null ? p.Action.Name : null, // ⬅️ Name, ne Title
                RegistrationDate = p.RegistrationDate
            })
            .FirstOrDefaultAsync(ct);

        if (dto is null)
            throw new MarketNotFoundException($"ActionParticipant with Id {request.Id} not found.");

        return dto;
    }
}
