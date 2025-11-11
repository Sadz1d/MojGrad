using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Application.Modules.Reports.Rating.Commands.Delete;

namespace Market.Application.Modules.Reports.Rating.Commands.Delete;

public sealed class DeleteRatingCommandHandler
    : IRequestHandler<DeleteRatingCommand, Unit>
{
    private readonly IAppDbContext _ctx;
    public DeleteRatingCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(DeleteRatingCommand request, CancellationToken ct)
    {
        var entity = await _ctx.Ratings.FirstOrDefaultAsync(r => r.Id == request.Id, ct);
        if (entity is null)
            throw new MarketNotFoundException($"Rating (Id={request.Id}) not found.");

        _ctx.Ratings.Remove(entity);
        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

