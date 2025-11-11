using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Reports;
using Market.Application.Modules.Reports.ProblemStatus.Commands.Create;

namespace Market.Application.Modules.Reports.ProblemStatus.Commands.Create;

public sealed class CreateProblemStatusCommandHandler
    : IRequestHandler<CreateProblemStatusCommand, int>
{
    private readonly IAppDbContext _ctx;
    public CreateProblemStatusCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<int> Handle(CreateProblemStatusCommand request, CancellationToken ct)
    {
        var name = request.Name.Trim();

        if (name.Length > ProblemStatusEntity.Constraints.NameMaxLength)
            throw new ArgumentException($"Name max length is {ProblemStatusEntity.Constraints.NameMaxLength}.");

        var exists = await _ctx.ProblemStatuses.AnyAsync(s => s.Name == name, ct);
        if (exists)
            throw new MarketConflictException($"ProblemStatus '{name}' already exists.");

        var entity = new ProblemStatusEntity
        {
            Name = name
        };

        _ctx.ProblemStatuses.Add(entity);
        await _ctx.SaveChangesAsync(ct);
        return entity.Id;
    }
}

