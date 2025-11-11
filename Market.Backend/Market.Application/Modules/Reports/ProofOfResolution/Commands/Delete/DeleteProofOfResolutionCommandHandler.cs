using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Application.Modules.Reports.ProofOfResolution.Commands.Delete;

namespace Market.Application.Modules.Reports.ProofOfResolution.Commands.Delete;

public sealed class DeleteProofOfResolutionCommandHandler
    : IRequestHandler<DeleteProofOfResolutionCommand, Unit>
{
    private readonly IAppDbContext _ctx;
    public DeleteProofOfResolutionCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(DeleteProofOfResolutionCommand request, CancellationToken ct)
    {
        var entity = await _ctx.ProofsOfResolution
            .FirstOrDefaultAsync(p => p.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"ProofOfResolution (Id={request.Id}) not found.");

        _ctx.ProofsOfResolution.Remove(entity);
        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

