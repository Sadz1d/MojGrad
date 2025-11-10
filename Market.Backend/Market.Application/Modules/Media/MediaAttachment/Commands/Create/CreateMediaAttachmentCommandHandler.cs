using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Media;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Media.Commands.Create;

public sealed class CreateMediaAttachmentCommandHandler
    : IRequestHandler<CreateMediaAttachmentCommand, int>
{
    private readonly IAppDbContext _ctx;

    public CreateMediaAttachmentCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<int> Handle(CreateMediaAttachmentCommand request, CancellationToken ct)
    {
        var normalizedUrl = request.FileUrl.Trim();

        // (opcija) zabrani duplikat URL-a za istog korisnika
        var exists = await _ctx.MediaAttachments
            .AnyAsync(m => m.UploaderId == request.UploaderId && m.FileUrl == normalizedUrl, ct);

        if (exists)
            throw new MarketConflictException(
                "Media attachment with the same URL already exists for this user.");

        var entity = new MediaAttachmentEntity
        {
            UploaderId = request.UploaderId,
            FileUrl = normalizedUrl,
            MimeType = request.MimeType.Trim(),
            SizeBytes = request.SizeBytes,
            CreatedAt = DateTime.UtcNow
        };

        _ctx.MediaAttachments.Add(entity);
        await _ctx.SaveChangesAsync(ct);

        return entity.Id;
    }
}

