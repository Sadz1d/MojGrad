using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Application.Modules.Reports.Rating.Queries.GetById;

namespace Market.Application.Modules.Reports.Rating.Queries.GetById;

public sealed class GetRatingByIdQueryHandler
    : IRequestHandler<GetRatingByIdQuery, GetRatingByIdQueryDto>
{
    private readonly IAppDbContext _ctx;
    public GetRatingByIdQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<GetRatingByIdQueryDto> Handle(
        GetRatingByIdQuery request, CancellationToken ct)
    {
        var dto = await _ctx.Ratings
            .AsNoTracking()
            .Include(r => r.User)
            .Where(r => r.Id == request.Id)
            .Select(r => new GetRatingByIdQueryDto
            {
                Id = r.Id,
                UserId = r.UserId,
                ReportId = r.ReportId,
                Rating = r.Rating,
                RatingComment = r.RatingComment,
                UserName = r.User != null
                    ? (r.User.FirstName + " " + r.User.LastName).Trim()
                    : "Anonimno"
            })
            .FirstOrDefaultAsync(ct);

        if (dto is null)
            throw new MarketNotFoundException($"Rating (Id={request.Id}) not found.");

        return dto;
    }
}
