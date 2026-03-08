using MediatR;
using Market.Application.Modules.Reports.ProblemReport.Dtos;
using Microsoft.AspNetCore.Http;

namespace Market.Application.Modules.Reports.ProblemReport.Commands.Import;

public class ImportProblemReportsCommand : IRequest<ImportProblemReportsResult>
{
    // Option A: parsed items (kept for backward compatibility)
    public List<ImportProblemReportDto>? Items { get; set; }

    // Option B: raw file — handler will parse it
    public IFormFile? File { get; set; }
    public bool SkipFirstRow { get; set; } = true;

    public bool DryRun { get; set; } = false;
}