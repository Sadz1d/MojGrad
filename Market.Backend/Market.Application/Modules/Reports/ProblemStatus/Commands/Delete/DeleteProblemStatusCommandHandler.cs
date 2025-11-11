using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Application.Modules.Reports.ProblemStatus.Commands.Delete;

namespace Market.Application.Modules.Reports.ProblemStatus.Commands.Delete;

public sealed class DeleteProblemStatusCommandHandler
    : IRequestHandler<DeleteProblemStatusCommand, Unit>
{
    private readonly IAppDbContext _ctx;
    public DeleteProblemStatusCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(DeleteProblemStatusCommand request, CancellationToken ct)
    {
        var entity = await _ctx.ProblemStatuses
            .FirstOrDefaultAsync(s => s.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"ProblemStatus (Id={request.Id}) not found.");

        _ctx.ProblemStatuses.Remove(entity);
        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

