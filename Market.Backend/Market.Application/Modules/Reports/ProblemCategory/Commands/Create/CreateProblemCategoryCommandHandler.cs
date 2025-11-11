using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Reports;
using Market.Application.Modules.Reports.ProblemCategory.Commands.Create;

namespace Market.Application.Modules.Reports.ProblemCategory.Commands.Create;

public sealed class CreateProblemCategoryCommandHandler
    : IRequestHandler<CreateProblemCategoryCommand, int>
{
    private readonly IAppDbContext _ctx;
    public CreateProblemCategoryCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<int> Handle(CreateProblemCategoryCommand request, CancellationToken ct)
    {
        var normalizedName = request.Name.Trim();

        var exists = await _ctx.ProblemCategories
            .AnyAsync(c => c.Name == normalizedName, ct);

        if (exists)
            throw new MarketConflictException("Problem category with this name already exists.");

        if (normalizedName.Length > ProblemCategoryEntity.Constraints.NameMaxLength)
            throw new ArgumentException($"Name max length is {ProblemCategoryEntity.Constraints.NameMaxLength}.");

        var entity = new ProblemCategoryEntity
        {
            Name = normalizedName,
            Description = request.Description?.Trim()
        };

        _ctx.ProblemCategories.Add(entity);
        await _ctx.SaveChangesAsync(ct);

        return entity.Id;
    }
}

