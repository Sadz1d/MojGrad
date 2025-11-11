using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Reports;
using Market.Application.Modules.Reports.ProblemStatus.Commands.Update;

namespace Market.Application.Modules.Reports.ProblemStatus.Commands.Update;

public sealed class UpdateProblemStatusCommandHandler
    : IRequestHandler<UpdateProblemStatusCommand, Unit>
{
    private readonly IAppDbContext _ctx;
    public UpdateProblemStatusCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(UpdateProblemStatusCommand request, CancellationToken ct)
    {
        var entity = await _ctx.ProblemStatuses
            .FirstOrDefaultAsync(s => s.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"ProblemStatus (Id={request.Id}) not found.");

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var name = request.Name.Trim();

            if (name.Length > ProblemStatusEntity.Constraints.NameMaxLength)
                throw new ArgumentException($"Name max length is {ProblemStatusEntity.Constraints.NameMaxLength}.");

            var exists = await _ctx.ProblemStatuses
                .AnyAsync(s => s.Id != request.Id && s.Name == name, ct);

            if (exists)
                throw new MarketConflictException($"Another status with name '{name}' already exists.");

            entity.Name = name;
        }

        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

