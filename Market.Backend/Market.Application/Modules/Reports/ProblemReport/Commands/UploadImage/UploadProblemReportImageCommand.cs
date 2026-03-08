using MediatR;
using Microsoft.AspNetCore.Http;

namespace Market.Application.Modules.Reports.ProblemReport.Commands.UploadImage;

public sealed class UploadProblemReportImageCommand : IRequest<string>
{
    public int ReportId { get; init; }
    public IFormFile Image { get; init; } = null!;
}