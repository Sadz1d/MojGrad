using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Abstractions;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Identity.Profiles.Commands.UploadPicture;

public sealed class UploadProfilePictureCommandHandler
    : IRequestHandler<UploadProfilePictureCommand, string>
{
    private readonly IAppDbContext _ctx;

    public UploadProfilePictureCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<string> Handle(UploadProfilePictureCommand request, CancellationToken ct)
    {
        var profile = await _ctx.Profiles
            .FirstOrDefaultAsync(p => p.UserId == request.UserId, ct)
            ?? throw new MarketNotFoundException($"Profile for UserId {request.UserId} not found.");

        var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var extension = Path.GetExtension(request.Image.FileName).ToLower();
        if (!allowed.Contains(extension))
            throw new MarketConflictException("Nepodržan format. Koristite JPG, PNG, GIF ili WebP.");

        // Delete old picture if exists
        if (!string.IsNullOrEmpty(profile.ProfilePicture))
        {
            var oldPath = Path.Combine(Directory.GetCurrentDirectory(), profile.ProfilePicture.TrimStart('/'));
            if (File.Exists(oldPath))
                File.Delete(oldPath);
        }

        var uploadsRoot = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Profiles");
        if (!Directory.Exists(uploadsRoot))
            Directory.CreateDirectory(uploadsRoot);

        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsRoot, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await request.Image.CopyToAsync(stream, ct);

        var relativePath = $"/Uploads/Profiles/{fileName}";
        profile.ProfilePicture = relativePath;
        await _ctx.SaveChangesAsync(ct);

        return relativePath;
    }
}