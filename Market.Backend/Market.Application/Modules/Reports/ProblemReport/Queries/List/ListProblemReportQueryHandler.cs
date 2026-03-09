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

        var projected = q.Select(p => new ListProblemReportQueryDto
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

        bool asc = string.Equals(request.SortDirection, "asc", StringComparison.OrdinalIgnoreCase);

        projected = (request.SortBy?.ToLower()) switch
        {
            "id" => asc ? projected.OrderBy(x => x.Id) : projected.OrderByDescending(x => x.Id),
            "title" => asc ? projected.OrderBy(x => x.Title) : projected.OrderByDescending(x => x.Title),
            "authorname" => asc ? projected.OrderBy(x => x.AuthorName) : projected.OrderByDescending(x => x.AuthorName),
            "categoryname" => asc ? projected.OrderBy(x => x.CategoryName) : projected.OrderByDescending(x => x.CategoryName),
            "statusname" => asc ? projected.OrderBy(x => x.StatusName) : projected.OrderByDescending(x => x.StatusName),
            "location" => asc ? projected.OrderBy(x => x.Location) : projected.OrderByDescending(x => x.Location),
            "creationdate" => asc ? projected.OrderBy(x => x.CreationDate) : projected.OrderByDescending(x => x.CreationDate),
            "commentscount" => asc ? projected.OrderBy(x => x.CommentsCount) : projected.OrderByDescending(x => x.CommentsCount),
            _ => projected.OrderByDescending(x => x.Id)
        };

        return await PageResult<ListProblemReportQueryDto>
            .FromQueryableAsync(projected, request.Paging, ct);
    }
}