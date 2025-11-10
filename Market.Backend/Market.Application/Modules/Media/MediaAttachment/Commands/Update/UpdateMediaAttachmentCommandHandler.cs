using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Media;

namespace Market.Application.Modules.Media.Commands.Update;

public sealed class UpdateMediaAttachmentCommandHandler
    : IRequestHandler<UpdateMediaAttachmentCommand, Unit>
{
    private readonly IAppDbContext _ctx;

    public UpdateMediaAttachmentCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<Unit> Handle(UpdateMediaAttachmentCommand request, CancellationToken ct)
    {
        var entity = await _ctx.MediaAttachments
            .FirstOrDefaultAsync(m => m.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"MediaAttachment (Id={request.Id}) not found.");

        // Ako postoji drugi media fajl s istim URL-om od istog korisnika (osim ovog)
        if (!string.IsNullOrWhiteSpace(request.FileUrl))
        {
            var normalizedUrl = request.FileUrl.Trim();

            var exists = await _ctx.MediaAttachments
                .AnyAsync(m => m.Id != request.Id
                            && m.UploaderId == entity.UploaderId
                            && m.FileUrl == normalizedUrl, ct);

            if (exists)
                throw new MarketConflictException(
                    "A media attachment with the same URL already exists for this user.");

            entity.FileUrl = normalizedUrl;
        }

        // Ažuriraj ostala polja samo ako su poslata
        if (!string.IsNullOrWhiteSpace(request.MimeType))
            entity.MimeType = request.MimeType.Trim();

        if (request.SizeBytes.HasValue)
            entity.SizeBytes = request.SizeBytes.Value;

        await _ctx.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

