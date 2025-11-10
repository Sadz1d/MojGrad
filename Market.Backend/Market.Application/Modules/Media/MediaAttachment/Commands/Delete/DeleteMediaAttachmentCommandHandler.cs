using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Media;

namespace Market.Application.Modules.Media.Commands.Delete;

public sealed class DeleteMediaAttachmentCommandHandler
    : IRequestHandler<DeleteMediaAttachmentCommand, Unit>
{
    private readonly IAppDbContext _ctx;

    public DeleteMediaAttachmentCommandHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<Unit> Handle(DeleteMediaAttachmentCommand request, CancellationToken ct)
    {
        var entity = await _ctx.MediaAttachments
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"Media attachment with Id {request.Id} not found.");

        _ctx.MediaAttachments.Remove(entity);
        await _ctx.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
