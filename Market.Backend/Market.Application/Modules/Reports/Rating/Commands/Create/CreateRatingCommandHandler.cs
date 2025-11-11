using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Reports;
using Market.Application.Modules.Reports.Rating.Commands.Create;

namespace Market.Application.Modules.Reports.Rating.Commands.Create;

public sealed class CreateRatingCommandHandler
    : IRequestHandler<CreateRatingCommand, int>
{
    private readonly IAppDbContext _ctx;
    public CreateRatingCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<int> Handle(CreateRatingCommand request, CancellationToken ct)
    {
        if (request.Rating < 1 || request.Rating > 5)
            throw new ArgumentException("Rating must be between 1 and 5.");

        var exists = await _ctx.ProblemReports.AnyAsync(p => p.Id == request.ReportId, ct);
        if (!exists)
            throw new MarketNotFoundException($"ProblemReport (Id={request.ReportId}) not found.");

        var userExists = await _ctx.Users.AnyAsync(u => u.Id == request.UserId, ct);
        if (!userExists)
            throw new MarketNotFoundException($"User (Id={request.UserId}) not found.");

        var entity = new RatingEntity
        {
            UserId = request.UserId,
            ReportId = request.ReportId,
            Rating = request.Rating,
            RatingComment = request.RatingComment?.Trim()
        };

        _ctx.Ratings.Add(entity);
        await _ctx.SaveChangesAsync(ct);

        return entity.Id;
    }
}

