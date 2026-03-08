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
        if (request.BiographyText != null) profile.BiographyText = request.BiographyText;

        if (request.ClearProfilePicture)
        {
            // Delete old file from disk if it exists
            if (!string.IsNullOrEmpty(profile.ProfilePicture))
            {
                var oldPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    profile.ProfilePicture.TrimStart('/'));
                if (File.Exists(oldPath))
                    File.Delete(oldPath);
            }
            profile.ProfilePicture = null;
        }
        else if (request.ProfilePicture != null)
        {
            profile.ProfilePicture = request.ProfilePicture;
        }

        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}