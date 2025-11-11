using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace Market.Application.Modules.Reports.ProblemReport.Commands.Create;

public sealed class CreateProblemReportCommand : IRequest<int>
{
    public required string Title { get; init; }
    public required int UserId { get; init; }
    public required string Description { get; init; }
    public string? Location { get; init; }
    public required int CategoryId { get; init; }
    public required int StatusId { get; init; }
}

