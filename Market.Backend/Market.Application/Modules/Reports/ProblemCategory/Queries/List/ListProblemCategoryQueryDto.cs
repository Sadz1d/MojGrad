using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Reports.ProblemCategory.Queries.List;

public sealed class ListProblemCategoryQueryDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public int ReportCount { get; init; }
}

