using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Application.Modules.Reports.ProblemReport.Queries.GetById;

namespace Market.Application.Modules.Reports.ProblemReport.Queries.GetById;

public sealed class GetProblemReportByIdQueryHandler
    : IRequestHandler<GetProblemReportByIdQuery, GetProblemReportByIdQueryDto>
{
    private readonly IAppDbContext _ctx;
    public GetProblemReportByIdQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<GetProblemReportByIdQueryDto> Handle(
        GetProblemReportByIdQuery request, CancellationToken ct)
    {
        var dto = await _ctx.ProblemReports
            .AsNoTracking()
            .Include(p => p.User)
            .Include(p => p.Category)
            .Include(p => p.Status)
            .Include(p => p.Comments)
            .Include(p => p.Tasks)
            .Include(p => p.Ratings)
            .Where(p => p.Id == request.Id)
            .Select(p => new GetProblemReportByIdQueryDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Location = p.Location,
                CreationDate = p.CreationDate,
                UserId = p.UserId,
                AuthorName = p.User != null
                    ? (p.User.FirstName + " " + p.User.LastName).Trim()
                    : "Anonimno",
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                StatusId = p.StatusId,
                StatusName = p.Status.Name,
                CommentsCount = p.Comments.Count,
                TasksCount = p.Tasks.Count,
                RatingsCount = p.Ratings.Count,
                ImagePath = p.ImagePath

            })
            .FirstOrDefaultAsync(ct);

        if (dto is null)
            throw new MarketNotFoundException($"ProblemReport (Id={request.Id}) not found.");

        return dto;
    }
}

