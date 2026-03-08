using MediatR;

namespace Market.Application.Modules.Reports.ProblemReport.Queries.GetImage;

public sealed class GetProblemReportImageQuery : IRequest<ProblemReportImageResult>
{
    public int ReportId { get; init; }
}

public sealed class ProblemReportImageResult
{
    public string FilePath { get; init; } = string.Empty;
    public string MimeType { get; init; } = string.Empty;
}