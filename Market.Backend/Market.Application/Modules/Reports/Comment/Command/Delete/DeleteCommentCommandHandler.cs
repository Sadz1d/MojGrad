using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Reports.Comments.Commands.Delete;

public sealed class DeleteCommentCommandHandler
    : IRequestHandler<DeleteCommentCommand, Unit>
{
    private readonly IAppDbContext _ctx;
    public DeleteCommentCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(DeleteCommentCommand request, CancellationToken ct)
    {
        var entity = await _ctx.Comments.FirstOrDefaultAsync(c => c.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"Comment (Id={request.Id}) not found.");

        _ctx.Comments.Remove(entity);
        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

