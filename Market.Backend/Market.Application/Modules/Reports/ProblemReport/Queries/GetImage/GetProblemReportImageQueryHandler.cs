using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Abstractions;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Reports.ProblemReport.Queries.GetImage;

public sealed class GetProblemReportImageQueryHandler
    : IRequestHandler<GetProblemReportImageQuery, ProblemReportImageResult>
{
    private readonly IAppDbContext _ctx;

    public GetProblemReportImageQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<ProblemReportImageResult> Handle(
        GetProblemReportImageQuery request, CancellationToken ct)
    {
        var report = await _ctx.ProblemReports
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == request.ReportId, ct)
            ?? throw new MarketNotFoundException($"Problem report with Id {request.ReportId} not found.");

        if (string.IsNullOrEmpty(report.ImagePath))
            throw new MarketNotFoundException("Ovaj izvještaj nema sliku.");

        var absolutePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            report.ImagePath.TrimStart('/'));

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

        return new ProblemReportImageResult { FilePath = absolutePath, MimeType = mime };
    }
}