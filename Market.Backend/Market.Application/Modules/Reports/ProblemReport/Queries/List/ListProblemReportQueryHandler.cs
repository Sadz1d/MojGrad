using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;

using Market.Domain.Entities.Reports;
using Market.Application.Modules.Reports.ProblemReport.Queries.List;

namespace Market.Application.Modules.Reports.ProblemReport.Queries.List;
public sealed class ListProblemReportsQueryHandler
    : IRequestHandler<ListProblemReportQuery, PageResult<ListProblemReportQueryDto>>
{
    private readonly IAppDbContext _ctx;
    public ListProblemReportsQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<PageResult<ListProblemReportQueryDto>> Handle(
        ListProblemReportQuery request, CancellationToken ct)
    {
        IQueryable<ProblemReportEntity> q = _ctx.ProblemReports
            .AsNoTracking()
            .Include(p => p.User)
            .Include(p => p.Category)
            .Include(p => p.Status)
            .Include(p => p.Comments)
            .Include(p => p.Tasks)
            .Include(p => p.Ratings);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            q = q.Where(p => p.Title.ToLower().Contains(term) ||
                             p.Description.ToLower().Contains(term));
        }
        if (request.UserId.HasValue)
            q = q.Where(p => p.UserId == request.UserId.Value);

        if (request.CategoryId.HasValue)
            q = q.Where(p => p.CategoryId == request.CategoryId.Value);

        if (request.StatusId.HasValue)
            q = q.Where(p => p.StatusId == request.StatusId.Value);

        var projected = q
            .OrderByDescending(p => p.CreationDate)
            .Select(p => new ListProblemReportQueryDto
            {
                Id = p.Id,
                Title = p.Title,
                AuthorName = p.User != null
                    ? (p.User.FirstName + " " + p.User.LastName).Trim()
                    : "Anonimno",
                CreationDate = p.CreationDate,
                Location = p.Location,
                CategoryName = p.Category.Name,
                StatusName = p.Status.Name,
                CommentsCount = p.Comments.Count,
                TasksCount = p.Tasks.Count,
                RatingsCount = p.Ratings.Count,
                ShortDescription = p.Description.Length > 160
                    ? p.Description.Substring(0, 160) + "..."
                    : p.Description
            });

        return await PageResult<ListProblemReportQueryDto>
            .FromQueryableAsync(projected, request.Paging, ct);
    }
}

