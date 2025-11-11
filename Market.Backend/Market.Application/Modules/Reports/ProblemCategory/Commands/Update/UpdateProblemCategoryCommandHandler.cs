using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Application.Modules.Reports.ProblemCategory.Commands.Update;
using Market.Domain.Entities.Reports;

namespace Market.Application.Modules.Reports.ProblemCategory.Commands.Update;

public sealed class UpdateProblemCategoryCommandHandler
    : IRequestHandler<UpdateProblemCategoryCommand, Unit>
{
    private readonly IAppDbContext _ctx;
    public UpdateProblemCategoryCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(UpdateProblemCategoryCommand request, CancellationToken ct)
    {
        var entity = await _ctx.ProblemCategories
            .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"ProblemCategory (Id={request.Id}) not found.");

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var name = request.Name.Trim();

            if (name.Length > 100)
                throw new ArgumentException($"Name max length is {ProblemCategoryEntity.Constraints.NameMaxLength}.");

            var exists = await _ctx.ProblemCategories
                .AnyAsync(c => c.Id != request.Id && c.Name == name, ct);

            if (exists)
                throw new MarketConflictException("Another category with the same name already exists.");

            entity.Name = name;
        }

        if (request.Description is not null)
            entity.Description = request.Description.Trim();

        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

