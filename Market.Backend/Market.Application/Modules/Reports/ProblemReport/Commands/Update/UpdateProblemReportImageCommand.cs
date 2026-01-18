using MediatR;

namespace Market.Application.Modules.Reports.ProblemReport.Commands.Update;

public sealed class UpdateProblemReportImageCommand : IRequest
{
    public int ProblemReportId { get; set; }
    public string ImagePath { get; set; } = string.Empty;
}
