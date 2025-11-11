using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace Market.Application.Modules.Reports.ProblemReport.Commands.Delete;

public sealed class DeleteProblemReportCommand : IRequest<Unit>
{
    public required int Id { get; init; }
}

