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

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; } = "desc";
}

