using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using System.Text.Json.Serialization;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Reports.Comments.Commands.Update;

public sealed class UpdateCommentCommandHandler
    : IRequestHandler<UpdateCommentCommand, Unit>
{
    private readonly IAppDbContext _ctx;
    public UpdateCommentCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(UpdateCommentCommand request, CancellationToken ct)
    {
        var entity = await _ctx.Comments.FirstOrDefaultAsync(c => c.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"Comment (Id={request.Id}) not found.");

        if (!string.IsNullOrWhiteSpace(request.Text))
            entity.Text = request.Text.Trim();

        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}


