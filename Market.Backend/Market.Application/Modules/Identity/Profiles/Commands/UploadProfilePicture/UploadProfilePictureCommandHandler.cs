using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Abstractions;
using Market.Application.Common.Exceptions;
using Market.Application.Common.Helpers;

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
            throw new MarketNotFoundException("Nepodržan format. Koristite JPG, PNG, GIF ili WebP.");

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

        // Always save as .jpg after compression
        var fileName = $"{Guid.NewGuid()}.jpg";
        var filePath = Path.Combine(uploadsRoot, fileName);

        using var inputStream = request.Image.OpenReadStream();
        // Profile pictures: max 400x400px, 80% quality
        // A 4MB phone photo typically becomes ~30-50KB
        using var compressed = await ImageCompressor.CompressAsync(
            inputStream, maxWidth: 400, maxHeight: 400, quality: 80);

        using var fileStream = new FileStream(filePath, FileMode.Create);
        await compressed.CopyToAsync(fileStream, ct);

        var relativePath = $"/Uploads/Profiles/{fileName}";
        profile.ProfilePicture = relativePath;
        await _ctx.SaveChangesAsync(ct);

        return relativePath;
    }
}