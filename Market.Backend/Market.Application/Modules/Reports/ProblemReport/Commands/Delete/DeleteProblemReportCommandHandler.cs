using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;

using Market.Application.Common.Exceptions;
using Market.Application.Modules.Reports.ProblemReport.Commands.Delete;

namespace Market.Application.Modules.Reports.ProblemReport.Commands.Delete;

public sealed class DeleteProblemReportCommandHandler
    : IRequestHandler<DeleteProblemReportCommand, Unit>
{
    private readonly IAppDbContext _ctx;
    public DeleteProblemReportCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(DeleteProblemReportCommand request, CancellationToken ct)
    {
        var entity = await _ctx.ProblemReports.FirstOrDefaultAsync(p => p.Id == request.Id, ct);
        if (entity is null)
            throw new MarketNotFoundException($"ProblemReport (Id={request.Id}) not found.");

        _ctx.ProblemReports.Remove(entity);
        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

