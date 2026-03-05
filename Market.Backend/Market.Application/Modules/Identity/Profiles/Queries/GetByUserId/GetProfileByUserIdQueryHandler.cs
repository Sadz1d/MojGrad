using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Abstractions;
using Market.Application.Common.Exceptions;
using Market.Application.Modules.Identity.Profiles.Queries.GetById;

namespace Market.Application.Modules.Identity.Profiles.Queries.GetByUserId;

public sealed class GetProfileByUserIdQueryHandler
    : IRequestHandler<GetProfileByUserIdQuery, GetProfileByIdQueryDto>
{
    private readonly IAppDbContext _ctx;
    public GetProfileByUserIdQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<GetProfileByIdQueryDto> Handle(
        GetProfileByUserIdQuery request, CancellationToken ct)
    {
        var profile = await _ctx.Profiles
            .Include(p => p.User)
            .ThenInclude(u => u.Reports)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == request.UserId, ct);

        if (profile == null)
            throw new MarketNotFoundException(
                $"Profile for UserId {request.UserId} not found.");

        return new GetProfileByIdQueryDto
        {
            Id = profile.Id,
            UserId = profile.UserId,
            Address = profile.Address,
            Phone = profile.Phone,
            ProfilePicture = profile.ProfilePicture,
            BiographyText = profile.BiographyText,
            UserFullName = (profile.User.FirstName + " " + profile.User.LastName).Trim(),
            Email = profile.User.Email,
            Points = profile.User.Points,
            RegistrationDate = profile.User.RegistrationDate,
            IsAdmin = profile.User.IsAdmin,
            IsManager = profile.User.IsManager,
            IsEmployee = profile.User.IsEmployee,
            ReportsCount = profile.User.Reports.Count
        };
    }
}