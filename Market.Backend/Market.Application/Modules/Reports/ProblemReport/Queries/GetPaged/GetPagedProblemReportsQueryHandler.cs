using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Abstractions;
using Market.Application.Common.Pagination;

namespace Market.Application.Modules.Reports.ProblemReport.Queries.GetPaged;

public sealed class GetPagedProblemReportsQueryHandler(
    IAppDbContext ctx)
    : IRequestHandler<GetPagedProblemReportsQuery, PagedResult<ProblemReportListItemDto>>
{
    public async Task<PagedResult<ProblemReportListItemDto>> Handle(
        GetPagedProblemReportsQuery request,
        CancellationToken ct)
    {
        var query = ctx.ProblemReports
            .AsNoTracking()
            .OrderByDescending(x => x.CreationDate);

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new ProblemReportListItemDto
            {
                Id = x.Id,
                Title = x.Title,
                CreatedAt = x.CreationDate,
                Status = x.Status.Name
            })
            .ToListAsync(ct);

        return new PagedResult<ProblemReportListItemDto>
        {
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            Items = items
        };
    }
}

