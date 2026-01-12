using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Market.Application.Common.Pagination;

namespace Market.Application.Modules.Reports.ProblemReport.Queries.GetPaged;

public sealed class GetPagedProblemReportsQuery
    : IRequest<PagedResult<ProblemReportListItemDto>>
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

