// Market.Application.Modules.Reports.ProblemReport.Queries.GetPaged.GetPagedProblemReportsQueryHandler
using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Abstractions;
using Market.Application.Common.Pagination;

namespace Market.Application.Modules.Reports.ProblemReport.Queries.GetPaged
{
    public sealed class GetPagedProblemReportsQueryHandler
        : IRequestHandler<GetPagedProblemReportsQuery, PagedResult<ProblemReportListItemDto>>
    {
        private readonly IAppDbContext _context;

        public GetPagedProblemReportsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<ProblemReportListItemDto>> Handle(
            GetPagedProblemReportsQuery request,
            CancellationToken ct)
        {
            // Osnovni query sa includes
            var query = _context.ProblemReports
                .AsNoTracking()
                .Include(pr => pr.User)
                .Include(pr => pr.Category)
                .Include(pr => pr.Status)
                .Include(pr => pr.Comments)
                .Include(pr => pr.Tasks)
                .Include(pr => pr.Ratings)
                .AsQueryable();

            // Primijeni filtere
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(pr =>
                    pr.Title.Contains(request.Search) ||
                    pr.Description.Contains(request.Search));
            }

            if (request.UserId.HasValue)
            {
                query = query.Where(pr => pr.UserId == request.UserId.Value);
            }

            if (request.CategoryId.HasValue)
            {
                query = query.Where(pr => pr.CategoryId == request.CategoryId.Value);
            }

            if (request.StatusId.HasValue)
            {
                query = query.Where(pr => pr.StatusId == request.StatusId.Value);
            }

            // Primijeni sortiranje
            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                query = request.SortBy.ToLower() switch
                {
                    "id" => request.SortDirection?.ToLower() == "asc"
                     ? query.OrderBy(pr => pr.Id)
                     : query.OrderByDescending(pr => pr.Id),
                    "title" => request.SortDirection?.ToLower() == "asc"
                        ? query.OrderBy(pr => pr.Title)
                        : query.OrderByDescending(pr => pr.Title),
                    "creationdate" or "createdat" => request.SortDirection?.ToLower() == "asc"
                        ? query.OrderBy(pr => pr.CreationDate)
                        : query.OrderByDescending(pr => pr.CreationDate),
                    "authorname" => request.SortDirection?.ToLower() == "asc"
                        ? query.OrderBy(pr => pr.User.FirstName)
                        : query.OrderByDescending(pr => pr.User.FirstName),
                    "categoryname" => request.SortDirection?.ToLower() == "asc"
                        ? query.OrderBy(pr => pr.Category.Name)
                        : query.OrderByDescending(pr => pr.Category.Name),
                    "statusname" or "status" => request.SortDirection?.ToLower() == "asc"
                        ? query.OrderBy(pr => pr.Status.Name)
                        : query.OrderByDescending(pr => pr.Status.Name),
                    "location" => request.SortDirection?.ToLower() == "asc"
                        ? query.OrderBy(pr => pr.Location)
                        : query.OrderByDescending(pr => pr.Location),
                    "commentscount" => request.SortDirection?.ToLower() == "asc"
                        ? query.OrderBy(pr => pr.Comments.Count)
                        : query.OrderByDescending(pr => pr.Comments.Count),
                    _ => query.OrderByDescending(pr => pr.CreationDate) // default
                };
            }
            else
            {
                query = query.OrderByDescending(pr => pr.CreationDate); // default sort
            }

            // Izračunaj ukupan broj
            var totalCount = await query.CountAsync(ct);

            // Primijeni paginaciju
            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(pr => new ProblemReportListItemDto
                {
                    Id = pr.Id,
                    Title = pr.Title,
                    AuthorName = pr.User != null ? pr.User.FirstName : "Nepoznato",
                    CreatedAt = pr.CreationDate,
                    Location = pr.Location,
                    CategoryName = pr.Category != null ? pr.Category.Name : "Nepoznato",
                    Status = pr.Status != null ? pr.Status.Name : "Nepoznato",
                    CommentsCount = pr.Comments.Count,
                    TasksCount = pr.Tasks.Count,
                    RatingsCount = pr.Ratings.Count,
                    ShortDescription = pr.Description.Length > 100
                        ? pr.Description.Substring(0, 100) + "..."
                        : pr.Description
                })
                .ToListAsync(ct);

            // Vrati rezultat
            return new PagedResult<ProblemReportListItemDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}