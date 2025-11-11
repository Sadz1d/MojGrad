using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Reports;

namespace Market.Application.Modules.Reports.Rating.Queries.List;

public sealed class ListRatingQueryHandler
    : IRequestHandler<ListRatingQuery, PageResult<ListRatingQueryDto>>
{
    private readonly IAppDbContext _ctx;
    public ListRatingQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<PageResult<ListRatingQueryDto>> Handle(
        ListRatingQuery request, CancellationToken ct)
    {
        IQueryable<RatingEntity> q = _ctx.Ratings
            .AsNoTracking()
            .Include(r => r.User)
            .Include(r => r.Report);

        if (request.ReportId.HasValue)
            q = q.Where(r => r.ReportId == request.ReportId.Value);

        if (request.UserId.HasValue)
            q = q.Where(r => r.UserId == request.UserId.Value);

        if (request.MinRating.HasValue)
            q = q.Where(r => r.Rating >= request.MinRating.Value);

        var projected = q
            .OrderByDescending(r => r.Rating)
            .Select(r => new ListRatingQueryDto
            {
                Id = r.Id,
                UserId = r.UserId,
                ReportId = r.ReportId,
                Rating = r.Rating,
                RatingComment = r.RatingComment,
                UserName = r.User != null
                    ? (r.User.FirstName + " " + r.User.LastName).Trim()
                    : "Anonimno"
            });

        return await PageResult<ListRatingQueryDto>
            .FromQueryableAsync(projected, request.Paging, ct);
    }
}

