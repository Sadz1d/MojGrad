using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Abstractions;
using Market.Application.Common.Exceptions;
using Market.Application.Common.Helpers;

namespace Market.Application.Modules.Reports.ProblemReport.Commands.UploadImage;

public sealed class UploadProblemReportImageCommandHandler
    : IRequestHandler<UploadProblemReportImageCommand, string>
{
    private readonly IAppDbContext _ctx;

    public UploadProblemReportImageCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<string> Handle(UploadProblemReportImageCommand request, CancellationToken ct)
    {
        var report = await _ctx.ProblemReports
            .FirstOrDefaultAsync(r => r.Id == request.ReportId, ct)
            ?? throw new MarketNotFoundException($"Problem report with Id {request.ReportId} not found.");

        var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var extension = Path.GetExtension(request.Image.FileName).ToLower();
        if (!allowed.Contains(extension))
            throw new MarketNotFoundException("Nepodržan format. Koristite JPG, PNG, GIF ili WebP.");

        var uploadsRoot = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "ProblemReports");
        if (!Directory.Exists(uploadsRoot))
            Directory.CreateDirectory(uploadsRoot);

        // Always save as .jpg after compression
        var fileName = $"{Guid.NewGuid()}.jpg";
        var filePath = Path.Combine(uploadsRoot, fileName);

        using var inputStream = request.Image.OpenReadStream();
        // Max 1200x1200px, 85% quality — keeps detail but much smaller file size
        using var compressed = await ImageCompressor.CompressAsync(
            inputStream, maxWidth: 1200, maxHeight: 1200, quality: 85);

        using var fileStream = new FileStream(filePath, FileMode.Create);
        await compressed.CopyToAsync(fileStream, ct);

        var relativePath = $"/Uploads/ProblemReports/{fileName}";
        report.ImagePath = relativePath;
        await _ctx.SaveChangesAsync(ct);

        return relativePath;
    }
}