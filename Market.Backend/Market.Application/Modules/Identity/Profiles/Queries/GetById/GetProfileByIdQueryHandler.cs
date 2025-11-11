using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Identity;

namespace Market.Application.Modules.Identity.Profiles.Queries.GetById;

public sealed class GetProfileByIdQueryHandler : IRequestHandler<GetProfileByIdQuery, GetProfileByIdQueryDto>
{
    private readonly IAppDbContext _ctx;

    public GetProfileByIdQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<GetProfileByIdQueryDto> Handle(GetProfileByIdQuery request, CancellationToken ct)
    {
        var profile = await _ctx.Profiles
            .Include(p => p.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.Id, ct);

        if (profile == null)
            throw new MarketNotFoundException($"Profile with Id {request.Id} not found.");

        return new GetProfileByIdQueryDto
        {
            Id = profile.Id,
            Address = profile.Address,
            Phone = profile.Phone,
            ProfilePicture = profile.ProfilePicture,
            BiographyText = profile.BiographyText,
            UserFullName = (profile.User.FirstName + " " + profile.User.LastName).Trim()
        };
    }
}
