using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Abstractions;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Identity.Profiles.Queries.GetPicture;

public sealed class GetProfilePictureQueryHandler
    : IRequestHandler<GetProfilePictureQuery, GetProfilePictureResult>
{
    private readonly IAppDbContext _ctx;

    public GetProfilePictureQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<GetProfilePictureResult> Handle(
        GetProfilePictureQuery request, CancellationToken ct)
    {
        var profile = await _ctx.Profiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == request.UserId, ct)
            ?? throw new MarketNotFoundException($"Profile for UserId {request.UserId} not found.");

        if (string.IsNullOrEmpty(profile.ProfilePicture))
            throw new MarketNotFoundException("Profilna slika nije postavljena.");

        var absolutePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            profile.ProfilePicture.TrimStart('/'));

        if (!File.Exists(absolutePath))
            throw new MarketNotFoundException("Fajl slike nije pronađen.");

        var mime = Path.GetExtension(absolutePath).ToLower() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            _ => "application/octet-stream"
        };

        return new GetProfilePictureResult { FilePath = absolutePath, MimeType = mime };
    }
}