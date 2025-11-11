using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;

using Market.Application.Common.Exceptions;
using Market.Application.Modules.Reports.ProofOfResolution.Commands.Update;

namespace Market.Application.Modules.Reports.ProofOfResolution.Commands.Update;

public sealed class UpdateProofOfResolutionCommandHandler
    : IRequestHandler<UpdateProofOfResolutionCommand, Unit>
{
    private readonly IAppDbContext _ctx;
    public UpdateProofOfResolutionCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(UpdateProofOfResolutionCommand request, CancellationToken ct)
    {
        var entity = await _ctx.ProofsOfResolution
            .FirstOrDefaultAsync(p => p.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"ProofOfResolution (Id={request.Id}) not found.");

        if (request.TaskId.HasValue)
        {
            var exists = await _ctx.Tasks.AnyAsync(t => t.Id == request.TaskId.Value, ct);
            if (!exists)
                throw new MarketNotFoundException($"Task (Id={request.TaskId.Value}) not found.");
            entity.TaskId = request.TaskId.Value;
        }

        if (request.UploadDate.HasValue)
            entity.UploadDate = request.UploadDate.Value;

        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

