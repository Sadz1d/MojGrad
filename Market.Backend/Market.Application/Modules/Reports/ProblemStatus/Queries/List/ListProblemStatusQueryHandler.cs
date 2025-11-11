using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;

using Market.Domain.Entities.Reports;
using Market.Application.Modules.Reports.ProblemStatus.Queries.List;

namespace Market.Application.Modules.Reports.ProblemStatus.Queries.List;
public sealed class ListProblemStatusQueryHandler
    : IRequestHandler<ListProblemStatusQuery, PageResult<ListProblemStatusQueryDto>>
{
    private readonly IAppDbContext _ctx;
    public ListProblemStatusQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<PageResult<ListProblemStatusQueryDto>> Handle(ListProblemStatusQuery request, CancellationToken ct)
    {
        IQueryable<ProblemStatusEntity> q = _ctx.ProblemStatuses
            .AsNoTracking()
            .Include(s => s.Reports);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            q = q.Where(s => s.Name.ToLower().Contains(term));
        }
        var projected = q
            .OrderBy(s => s.Name)
            .Select(s => new ListProblemStatusQueryDto
            {
                Id = s.Id,
                Name = s.Name,
                ReportCount = s.Reports.Count
            });

        return await PageResult<ListProblemStatusQueryDto>
            .FromQueryableAsync(projected, request.Paging, ct);
    }
}

