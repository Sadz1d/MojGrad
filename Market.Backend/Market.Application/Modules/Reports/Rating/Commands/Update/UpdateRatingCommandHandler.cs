using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Reports;
using Market.Application.Modules.Report.Ratings.Commands.Update;

namespace Market.Application.Modules.Reports.Rating.Commands.Update;

public sealed class UpdateRatingCommandHandler
    : IRequestHandler<UpdateRatingCommand, Unit>
{
    private readonly IAppDbContext _ctx;
    public UpdateRatingCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(UpdateRatingCommand request, CancellationToken ct)
    {
        var entity = await _ctx.Ratings.FirstOrDefaultAsync(r => r.Id == request.Id, ct);
        if (entity is null)
            throw new MarketNotFoundException($"Rating (Id={request.Id}) not found.");

        if (request.Rating.HasValue)
        {
            if (request.Rating.Value < 1 || request.Rating.Value > 5)
                throw new ArgumentException("Rating must be between 1 and 5.");
            entity.Rating = request.Rating.Value;
        }

        if (request.RatingComment is not null)
        {
            if (request.RatingComment.Length > RatingEntity.Constraints.CommentMaxLength)
                throw new ArgumentException($"Comment max length is {RatingEntity.Constraints.CommentMaxLength}.");
            entity.RatingComment = request.RatingComment.Trim();
        }

        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

