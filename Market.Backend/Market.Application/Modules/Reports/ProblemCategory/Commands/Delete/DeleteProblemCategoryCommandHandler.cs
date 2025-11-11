using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Application.Modules.Reports.ProblemCategory.Commands.Delete;

namespace Market.Application.Modules.Reports.ProblemCategory.Commands.Delete;

public sealed class DeleteProblemCategoryCommandHandler
    : IRequestHandler<DeleteProblemCategoryCommand, Unit>
{
    private readonly IAppDbContext _ctx;
    public DeleteProblemCategoryCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(DeleteProblemCategoryCommand request, CancellationToken ct)
    {
        var entity = await _ctx.ProblemCategories
            .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"ProblemCategory (Id={request.Id}) not found.");

        _ctx.ProblemCategories.Remove(entity);
        await _ctx.SaveChangesAsync(ct);

        return Unit.Value;
    }
}

