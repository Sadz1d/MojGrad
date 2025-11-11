using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;

using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Reports;
using Market.Application.Modules.Reports.ProblemReport.Commands.Update;

namespace Market.Application.Modules.Reports.ProblemReport.Commands.Update;

public sealed class UpdateProblemReportCommandHandler
    : IRequestHandler<UpdateProblemReportCommand, Unit>
{
    private readonly IAppDbContext _ctx;
    public UpdateProblemReportCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(UpdateProblemReportCommand request, CancellationToken ct)
    {
        var entity = await _ctx.ProblemReports
            .FirstOrDefaultAsync(p => p.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"ProblemReport (Id={request.Id}) not found.");

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            var t = request.Title.Trim();
            if (t.Length > ProblemReportEntity.Constraints.TitleMaxLength)
                throw new ArgumentException($"Title max length is {ProblemReportEntity.Constraints.TitleMaxLength}.");
            entity.Title = t;
        }

        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            var d = request.Description.Trim();
            if (d.Length > ProblemReportEntity.Constraints.DescriptionMaxLength)
                throw new ArgumentException($"Description max length is {ProblemReportEntity.Constraints.DescriptionMaxLength}.");
            entity.Description = d;
        }

        if (request.Location is not null)
        {
            var loc = request.Location.Trim();
            if (loc.Length > ProblemReportEntity.Constraints.LocationMaxLength)
                throw new ArgumentException($"Location max length is {ProblemReportEntity.Constraints.LocationMaxLength}.");
            entity.Location = loc;
        }

        if (request.CategoryId.HasValue)
        {
            var exists = await _ctx.ProblemCategories.AnyAsync(x => x.Id == request.CategoryId.Value, ct);
            if (!exists) throw new MarketNotFoundException($"ProblemCategory (Id={request.CategoryId.Value}) not found.");
            entity.CategoryId = request.CategoryId.Value;
        }

        if (request.StatusId.HasValue)
        {
            var exists = await _ctx.ProblemStatuses.AnyAsync(x => x.Id == request.StatusId.Value, ct);
            if (!exists) throw new MarketNotFoundException($"ProblemStatus (Id={request.StatusId.Value}) not found.");
            entity.StatusId = request.StatusId.Value;
        }

        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

