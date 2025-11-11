using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Application.Modules.Media.MediaLink.Commands.Delete;

namespace Market.Application.Modules.Media.MediaLink.Commands.Delete;

public sealed class DeleteMediaLinkCommandHandler
    : IRequestHandler<DeleteMediaLinkCommand, Unit>
{
    private readonly IAppDbContext _ctx;
    public DeleteMediaLinkCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(DeleteMediaLinkCommand request, CancellationToken ct)
    {
        var entity = await _ctx.MediaLinks
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"MediaLink (Id={request.Id}) not found.");

        _ctx.MediaLinks.Remove(entity);
        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

