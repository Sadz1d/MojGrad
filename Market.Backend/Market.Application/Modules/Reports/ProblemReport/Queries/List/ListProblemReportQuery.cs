using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Market.Application.Modules.Reports.ProblemReport.Queries.List;

namespace Market.Application.Modules.Reports.ProblemReport.Queries.List;

public sealed class ListProblemReportQuery : BasePagedQuery<ListProblemReportQueryDto>
{
    public string? Search { get; init; }     // po Title/Description
    public int? UserId { get; init; }        // autor
    public int? CategoryId { get; init; }    // kategorija
    public int? StatusId { get; init; }      // status
}

