using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Reports;

namespace Market.Application.Modules.Reports.ProblemCategory.Queries.List;

public sealed class ListProblemCategoryQueryHandler
    : IRequestHandler<ListProblemCategoryQuery, PageResult<ListProblemCategoryQueryDto>>
{
    private readonly IAppDbContext _ctx;
    public ListProblemCategoryQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<PageResult<ListProblemCategoryQueryDto>> Handle(ListProblemCategoryQuery request, CancellationToken ct)
    {
        IQueryable<ProblemCategoryEntity> q = _ctx.ProblemCategories
            .AsNoTracking()
            .Include(c => c.Reports);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            q = q.Where(c => c.Name.ToLower().Contains(term));
        }

        var projected = q
            .OrderBy(c => c.Name)
            .Select(c => new ListProblemCategoryQueryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ReportCount = c.Reports.Count
            });

        return await PageResult<ListProblemCategoryQueryDto>
            .FromQueryableAsync(projected, request.Paging, ct);
    }
}

