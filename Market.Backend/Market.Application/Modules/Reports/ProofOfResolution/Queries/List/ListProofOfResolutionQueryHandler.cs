using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Reports;
using Market.Application.Modules.Reports.ProofOfResolution.Queries.List;

namespace Market.Application.Modules.Reports.ProofsOfResolution.Queries.List;
public sealed class ListProofOfResolutionQueryHandler
    : IRequestHandler<ListProofOfResolutionQuery, PageResult<ListProofOfResolutionQueryDto>>
{
    private readonly IAppDbContext _ctx;
    public ListProofOfResolutionQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<PageResult<ListProofOfResolutionQueryDto>> Handle(
        ListProofOfResolutionQuery request, CancellationToken ct)
    {
        IQueryable<ProofOfResolutionEntity> q = _ctx.ProofsOfResolution
            .AsNoTracking()
            .Include(p => p.Task);

        if (request.TaskId.HasValue)
            q = q.Where(p => p.TaskId == request.TaskId.Value);

        if (request.From.HasValue)
            q = q.Where(p => p.UploadDate >= request.From.Value);

        if (request.To.HasValue)
        {
            var to = request.To.Value.Date.AddDays(1); // inclusive
            q = q.Where(p => p.UploadDate < to);
        }
        var projected = q
     .OrderByDescending(p => p.UploadDate)
     .Select(p => new ListProofOfResolutionQueryDto
     {
         Id = p.Id,
         TaskId = p.TaskId,
         UploadDate = p.UploadDate
     });

        return await PageResult<ListProofOfResolutionQueryDto>
            .FromQueryableAsync(projected, request.Paging, ct);
    }
}

