using MediatR;
using Market.Application.Modules.Reports.ProblemReport.Dtos;

namespace Market.Application.Modules.Reports.ProblemReport.Commands.Import;

public class ImportProblemReportsCommand : IRequest<ImportProblemReportsResult>
{
    public List<ImportProblemReportDto> Items { get; set; }
    public bool DryRun { get; set; } = false;
}