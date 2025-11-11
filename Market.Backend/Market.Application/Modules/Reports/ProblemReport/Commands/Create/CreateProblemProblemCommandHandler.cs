using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Reports;
using Market.Application.Modules.Reports.ProblemReport.Commands.Create;

namespace Market.Application.Modules.Reports.ProblemReport.Commands.Create;

public sealed class CreateProblemReportCommandHandler
    : IRequestHandler<CreateProblemReportCommand, int>
{
    private readonly IAppDbContext _ctx;
    public CreateProblemReportCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<int> Handle(CreateProblemReportCommand request, CancellationToken ct)
    {
        var title = request.Title.Trim();

        if (title.Length > ProblemReportEntity.Constraints.TitleMaxLength)
            throw new ArgumentException($"Title max length is {ProblemReportEntity.Constraints.TitleMaxLength}.");

        var desc = request.Description.Trim();
        if (desc.Length > ProblemReportEntity.Constraints.DescriptionMaxLength)
            throw new ArgumentException($"Description max length is {ProblemReportEntity.Constraints.DescriptionMaxLength}.");

        // Validacija FK
        var categoryExists = await _ctx.ProblemCategories.AnyAsync(x => x.Id == request.CategoryId, ct);
        if (!categoryExists) throw new MarketNotFoundException($"ProblemCategory (Id={request.CategoryId}) not found.");

        var statusExists = await _ctx.ProblemStatuses.AnyAsync(x => x.Id == request.StatusId, ct);
        if (!statusExists) throw new MarketNotFoundException($"ProblemStatus (Id={request.StatusId}) not found.");

        var entity = new ProblemReportEntity
        {
            Title = title,
            UserId = request.UserId,
            Description = desc,
            Location = request.Location?.Trim(),
            CategoryId = request.CategoryId,
            StatusId = request.StatusId,
            CreationDate = DateTime.UtcNow
        };

        _ctx.ProblemReports.Add(entity);
        await _ctx.SaveChangesAsync(ct);
        return entity.Id;
    }
}

