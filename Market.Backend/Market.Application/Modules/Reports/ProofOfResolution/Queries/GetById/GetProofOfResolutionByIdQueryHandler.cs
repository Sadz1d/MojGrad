using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Application.Modules.Reports.ProofOfResolution.Queries.GetById;

namespace Market.Application.Modules.Reports.ProofOfResolution.Queries.GetById;

public sealed class GetProofOfResolutionByIdQueryHandler
    : IRequestHandler<GetProofOfResolutionByIdQuery, GetProofOfResolutionByIdQueryDto>
{
    private readonly IAppDbContext _ctx;
    public GetProofOfResolutionByIdQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<GetProofOfResolutionByIdQueryDto> Handle(
        GetProofOfResolutionByIdQuery request, CancellationToken ct)
    {
        var dto = await _ctx.ProofsOfResolution
            .AsNoTracking()
            .Include(p => p.Task)
            .Where(p => p.Id == request.Id)
            .Select(p => new GetProofOfResolutionByIdQueryDto
            {
                Id = p.Id,
                TaskId = p.TaskId,
                UploadDate = p.UploadDate,
                
            })
            .FirstOrDefaultAsync(ct);

        if (dto is null)
            throw new MarketNotFoundException($"ProofOfResolution (Id={request.Id}) not found.");

        return dto;
    }
}

