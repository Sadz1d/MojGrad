using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Reports.ProblemStatus.Queries.List;

public sealed class ListProblemStatusQueryDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public int ReportCount { get; init; }
}

