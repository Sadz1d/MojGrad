using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Market.Application.Modules.Reports.ProblemCategory.Queries.List;

public sealed class ListProblemCategoryQuery : BasePagedQuery<ListProblemCategoryQueryDto>
{
    public string? Search { get; init; } // filtriranje po imenu
}

