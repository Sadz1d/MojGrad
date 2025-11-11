using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Application.Modules.Reports.ProblemCategory.Queries.GetById;

namespace Market.Application.Modules.Reports.ProblemCategory.Queries.GetById;

public sealed class GetProblemCategoryByIdQueryHandler
    : IRequestHandler<GetProblemCategoryByIdQuery, GetProblemCategoryByIdQueryDto>
{
    private readonly IAppDbContext _ctx;
    public GetProblemCategoryByIdQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<GetProblemCategoryByIdQueryDto> Handle(GetProblemCategoryByIdQuery request, CancellationToken ct)
    {
        var dto = await _ctx.ProblemCategories
            .AsNoTracking()
            .Include(c => c.Reports)
            .Where(c => c.Id == request.Id)
            .Select(c => new GetProblemCategoryByIdQueryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ReportCount = c.Reports.Count
            })
            .FirstOrDefaultAsync(ct);

        if (dto is null)
            throw new MarketNotFoundException($"ProblemCategory (Id={request.Id}) not found.");

        return dto;
    }
}

