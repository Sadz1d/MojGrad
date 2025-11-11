using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Identity;

namespace Market.Application.Modules.Identity.Profiles.Commands.Update;

public sealed class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Unit>
{
    private readonly IAppDbContext _ctx;

    public UpdateProfileCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(UpdateProfileCommand request, CancellationToken ct)
    {
        var profile = await _ctx.Profiles
            .FirstOrDefaultAsync(p => p.Id == request.Id, ct);

        if (profile == null)
            throw new MarketNotFoundException($"Profile with Id {request.Id} not found.");

        if (request.Address != null) profile.Address = request.Address.Trim();
        if (request.Phone != null) profile.Phone = request.Phone.Trim();
        if (request.ProfilePicture != null) profile.ProfilePicture = request.ProfilePicture;
        if (request.BiographyText != null) profile.BiographyText = request.BiographyText;

        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
