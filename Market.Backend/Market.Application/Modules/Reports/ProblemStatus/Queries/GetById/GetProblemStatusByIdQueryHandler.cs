using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Application.Modules.Reports.ProblemStatus.Queries.GetById;

namespace Market.Application.Modules.Reports.ProblemStatus.Queries.GetById;

public sealed class GetProblemStatusByIdQueryHandler
    : IRequestHandler<GetProblemStatusByIdQuery, GetProblemStatusByIdQueryDto>
{
    private readonly IAppDbContext _ctx;
    public GetProblemStatusByIdQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<GetProblemStatusByIdQueryDto> Handle(GetProblemStatusByIdQuery request, CancellationToken ct)
    {
        var dto = await _ctx.ProblemStatuses
            .AsNoTracking()
            .Include(s => s.Reports)
            .Where(s => s.Id == request.Id)
            .Select(s => new GetProblemStatusByIdQueryDto
            {
                Id = s.Id,
                Name = s.Name,
                ReportCount = s.Reports.Count
            })
            .FirstOrDefaultAsync(ct);

        if (dto is null)
            throw new MarketNotFoundException($"ProblemStatus (Id={request.Id}) not found.");

        return dto;
    }
}
