using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Reports;
using Market.Application.Modules.Reports.ProofOfResolution.Commands.Create;

namespace Market.Application.Modules.Reports.ProofOfResolution.Commands.Create;

public sealed class CreateProofOfResolutionCommandHandler
    : IRequestHandler<CreateProofOfResolutionCommand, int>
{
    private readonly IAppDbContext _ctx;
    public CreateProofOfResolutionCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<int> Handle(CreateProofOfResolutionCommand request, CancellationToken ct)
    {
        var taskExists = await _ctx.Tasks.AnyAsync(t => t.Id == request.TaskId, ct);
        if (!taskExists)
            throw new MarketNotFoundException($"Task (Id={request.TaskId}) not found.");

        var entity = new ProofOfResolutionEntity
        {
            TaskId = request.TaskId,
            UploadDate = request.UploadDate ?? DateTime.UtcNow
        };

        _ctx.ProofsOfResolution.Add(entity);
        await _ctx.SaveChangesAsync(ct);
        return entity.Id;
    }
}

